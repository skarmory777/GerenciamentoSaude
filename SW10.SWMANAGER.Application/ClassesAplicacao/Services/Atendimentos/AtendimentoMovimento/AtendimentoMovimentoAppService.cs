using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AtendimentoMovimento
{
    public class AtendimentoMovimentoAppService : SWMANAGERAppServiceBase, IAtendimentoMovimentoAppService
    {
        [UnitOfWork]
        public async Task<AtendimentoMovimentoDto> AssumirAtendimento(long atendimentoId)
        {
            using (var atendimentoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Atendimentos.Atendimentos.AtendimentoMovimento, long>>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Authorization.Users.User, long>>())
            {
                var usuario = usuarioRepository.Object.GetAll().FirstOrDefault(x => x.Id == this.AbpSession.UserId);

                if (!usuario.MedicoId.HasValue)
                {
                    return new AtendimentoMovimentoDto { AtendimentoId = atendimentoId };
                }

                await this.FinalizarAtendimento(atendimentoId).ConfigureAwait(false);

                var atendimentoMovimentoNovo = new ClassesAplicacao.Atendimentos.Atendimentos.AtendimentoMovimento
                {
                    AtendimentoId = atendimentoId,
                    DataInicio = DateTime.Now,
                    MedicoId = usuario.MedicoId
                };

                await atendimentoMovimentoRepository.Object.InsertOrUpdateAsync(atendimentoMovimentoNovo).ConfigureAwait(false);

                var atendimento = await atendimentoRepository.Object.FirstOrDefaultAsync(atendimentoId);

                if(atendimento != null && !atendimento.IsTransient())
                {
                    if (atendimento.AtendimentoStatusId == AtendimentoStatus.Aguardando)
                    {
                        atendimento.AtendimentoStatusId = AtendimentoStatus.EmAtendimento;
                    }

                    await atendimentoRepository.Object.UpdateAsync(atendimento);

                    if (await atendimentoMovimentoRepository.Object.GetAll().AsNoTracking().Where(x => x.AtendimentoId == atendimentoId).CountAsync() == 0)
                    {
                        await atendimentoAppService.Object.AlterarMedicoAtendimento(atendimentoId);
                    }
                }

                return new AtendimentoMovimentoDto
                {
                    AtendimentoId = atendimentoId,
                    DataInicio = atendimentoMovimentoNovo.DataInicio,
                    DataFinal = atendimentoMovimentoNovo.DataFinal,
                    MedicoId = atendimentoMovimentoNovo.MedicoId.Value
                };
            }
        }

        [UnitOfWork]
        public async Task<AtendimentoMovimentoDto> FinalizarAtendimento(long atendimentoId)
        {
            using (var atendimentoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Atendimentos.Atendimentos.AtendimentoMovimento, long>>())
            {
                var atendimentoMovimento = await atendimentoMovimentoRepository.Object.GetAll().Where(x => x.AtendimentoId == atendimentoId && !x.DataFinal.HasValue).OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                if (atendimentoMovimento != null)
                {
                    atendimentoMovimento.DataFinal = DateTime.Now;

                    await atendimentoMovimentoRepository.Object.InsertOrUpdateAsync(atendimentoMovimento).ConfigureAwait(false);

                    return new AtendimentoMovimentoDto
                    {
                        AtendimentoId = atendimentoId,
                        DataInicio = atendimentoMovimento.DataInicio,
                        DataFinal = atendimentoMovimento.DataFinal,
                        MedicoId = atendimentoMovimento.MedicoId.Value
                    };
                }

                return null;
            }
        }

        [UnitOfWork]
        public async Task<AtendimentoMovimentoDto> Obter(long atendimentoId)
        {
            using (var atendimentoMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ClassesAplicacao.Atendimentos.Atendimentos.AtendimentoMovimento, long>>())
            using (var atendimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Atendimento, long>>())
            using (var usuarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Authorization.Users.User, long>>())
            {
                try
                {
                    var usuario = usuarioRepository.Object.GetAll().FirstOrDefault(x => x.Id == this.AbpSession.UserId);

                    if (!usuario.MedicoId.HasValue)
                    {
                        return new AtendimentoMovimentoDto { AtendimentoId = atendimentoId };
                    }

                    var atendimentoMovimento = await atendimentoMovimentoRepository.Object.GetAll().AsNoTracking().Where(x => x.AtendimentoId == atendimentoId).OrderByDescending(x => x.Id).FirstOrDefaultAsync();
                    var atendimentoMovimentoDto = new AtendimentoMovimentoDto();
                    if (atendimentoMovimento != null)
                    {
                        return new AtendimentoMovimentoDto
                        {
                            AtendimentoId = atendimentoId,
                            AssumirAtendimento = atendimentoMovimento.MedicoId != usuario.MedicoId,
                            IniciarAtendimento = atendimentoMovimento.MedicoId == usuario.MedicoId && atendimentoMovimento.DataFinal.HasValue,
                            DataInicio = atendimentoMovimento.DataInicio,
                            DataFinal = atendimentoMovimento.DataFinal,
                            MedicoId = atendimentoMovimento.MedicoId.Value
                        };
                    }

                    var atendimento = await atendimentoRepository.Object.GetAll().AsNoTracking().Where(x => x.Id == atendimentoId).Select(x => new { x.MedicoId, x.Id }).FirstOrDefaultAsync();
                    return new AtendimentoMovimentoDto
                    {
                        AtendimentoId = atendimentoId,
                        AssumirAtendimento = atendimento?.MedicoId != usuario.MedicoId,
                        IniciarAtendimento = true,
                        DataInicio = DateTime.Now,
                        MedicoId = usuario.MedicoId.Value

                    };
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
    }
}
