namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Servicos
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    using Abp.Auditing;
    using Abp.Dependency;
    using Abp.Domain.Repositories;
    using Abp.Domain.Uow;
    using Abp.UI;

    using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Interfaces;
    using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;

    public class SenhaAppService : SWMANAGERAppServiceBase, ISenhaAppService
    {

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarSenhasPorlocalChamadaDropdown(DropdownInput dropdownInput)
        {
            using (var senhaMovimentacaoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
            using (var localChamadaRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<LocalChamada, long>>())
            {
                long localChamadaId;

                long.TryParse(dropdownInput.filtro, out localChamadaId);


                var localChamada = await localChamadaRepository.Object.GetAll().AsNoTracking().Include(i => i.TipoLocalChamada)
                                       .FirstOrDefaultAsync(w => w.Id == localChamadaId).ConfigureAwait(false);

                var dataDMenos1 = DateTime.Now.Date.AddDays(-1);

                return await this.ListarDropdownLambda(
                           dropdownInput,
                           senhaMovimentacaoRepository.Object,
                           m =>
                               // (string.IsNullOrEmpty(dropdownInput.search) || m.Senha.Descricao.ToLower().Contains(dropdownInput.search.ToLower())
                               //|| m.Senha.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower()))
                               // && 
                               m.TipoLocalChamadaId == localChamada.TipoLocalChamadaId && m.DataHoraFinal == null
                                                                                       && m.Senha.DataHora
                                                                                       > dataDMenos1,
                           p => new DropdownItems
                           {
                               id = p.Id,
                               text = p.Senha.Atendimento != null
                                                   ? p.Senha.Atendimento.Paciente.NomeCompleto
                                                   : p.Senha.Numero.ToString()
                           },
                           o => o.Senha.DataHora.ToString()).ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarSenhasNaoChamadas(DropdownInput dropdownInput)
        {
            using (var senhaMovimentacaoRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
            {
                var dataDMenos1 = DateTime.Now.Date.AddDays(-1);

                return await this.ListarDropdownLambda(
                           dropdownInput,
                           senhaMovimentacaoRepository.Object,
                           m => m.DataHoraFinal == null && m.Senha.DataHora > dataDMenos1,
                           p => new DropdownItems
                           {
                               id = p.Id,
                               text = p.Senha.Atendimento != null
                                                   ? p.Senha.Atendimento.Paciente.NomeCompleto
                                                   : p.Senha.Numero.ToString()
                           },
                           o => o.Senha.DataHora.ToString()).ConfigureAwait(false);
            }
        }

        public MonitorChamadaIndex CarregarPainelSenha(long painelId)
        {
            using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var senhaMovimentacaoPainelRepository =
                IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacaoPainel, long>>())
            {
                var movimentacaoPaineis = senhaMovimentacaoPainelRepository.Object.GetAll().AsNoTracking()
                    .Include(i => i.SenhaMovimentacao).Include(i => i.SenhaMovimentacao.LocalChamada)
                    .Include(i => i.SenhaMovimentacao.Senha).Include(i => i.SenhaMovimentacao.Senha.Atendimento)
                    .Include(i => i.SenhaMovimentacao.Senha.Atendimento.Paciente)
                    .Include(i => i.SenhaMovimentacao.Senha.Atendimento.Paciente.SisPessoa)
                    .Where(w => w.IsMostra && w.PainelId == painelId).OrderBy(o => o.Id).FirstOrDefault();



                if (movimentacaoPaineis == null || movimentacaoPaineis.SenhaMovimentacao?.LocalChamada == null || movimentacaoPaineis.SenhaMovimentacao?.Senha == null)
                {
                    return null;
                }

                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var length = movimentacaoPaineis.SenhaMovimentacao.Senha.Atendimento?.Paciente.NomeCompleto.Length;

                    var tamanho = (int)(length < 30 ? length : 30);

                    var monitorChamadaIndex = new MonitorChamadaIndex
                    {
                        LocalChamadaAtual = movimentacaoPaineis.SenhaMovimentacao.LocalChamada.Descricao,
                        SenhaAtual = movimentacaoPaineis.SenhaMovimentacao.Senha.Numero,
                        NomePaciente = movimentacaoPaineis.SenhaMovimentacao.Senha.Atendimento?.Paciente.NomeCompleto?.Substring(0, tamanho)
                    };

                    movimentacaoPaineis.IsMostra = false;

                    senhaMovimentacaoPainelRepository.Object.InsertOrUpdate(movimentacaoPaineis);

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                    return monitorChamadaIndex;
                }
            }
        }

        public async Task<SenhaDto> Obter(long id)
        {
            try
            {
                using (var senhaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Senha, long>>())
                {
                    var senha = await senhaRepository.Object.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
                    return SenhaDto.Mapear(senha); //senhaDto.MapTo<SenhaDto>();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<SenhaMovimentacaoDto> ObterMovimento(long id)
        {
            try
            {
                using (var senhaMovimentacaoRepository =
                    IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
                {
                    var senhaMovimentacao = await senhaMovimentacaoRepository.Object.GetAll().AsNoTracking()
                                                .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

                    return SenhaMovimentacaoDto.Mapear(senhaMovimentacao); //senha.MapTo<SenhaMovimentacaoDto>();
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork]
        public async Task CriarMovimento(long atendimentoId, long tipoLocalChamadaId)
        {
            try
            {
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var senhaMovimentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var movimentacaoSenha = senhaMovimentacaoRepository.Object.GetAll()
                                            .Include(i => i.Senha)
                                            .Include(i => i.TipoLocalChamada)
                                            .Where(w => w.Senha.AtendimentoId == atendimentoId)
                                            .ToList()
                                            .LastOrDefault();

                    if (movimentacaoSenha != null)
                    {
                        if (!movimentacaoSenha.DataHoraFinal.HasValue)
                        {
                            movimentacaoSenha.DataHoraFinal = DateTime.Now;
                        }

                        var senhaMovimentacao = new SenhaMovimentacao
                        {
                            SenhaId = movimentacaoSenha.Senha.Id,
                            TipoLocalChamadaId = tipoLocalChamadaId,
                            DataHora = DateTime.Now
                        };
                        await senhaMovimentacaoRepository.Object.InsertAsync(senhaMovimentacao).ConfigureAwait(false);
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarSenhasPorlocalChamadaAtendimentoDropdown(DropdownInput dropdownInput)
        {
            using (var localChamadaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LocalChamada, long>>())
            using (var senhaMovimentacaoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<SenhaMovimentacao, long>>())
            {
                long localChamadaId;
                long atendimentoId;

                long.TryParse(dropdownInput.filtros[0], out localChamadaId);
                long.TryParse(dropdownInput.filtros[1], out atendimentoId);


                var localChamada = localChamadaRepository.Object.GetAll().AsNoTracking().Include(i => i.TipoLocalChamada)
                    .FirstOrDefault(w => w.Id == localChamadaId);

                var dataDMenos1 = DateTime.Now.Date.AddDays(-1);

                var query = senhaMovimentacaoRepository.Object.GetAll().AsNoTracking()
                    .Where(w => w.Senha.AtendimentoId == atendimentoId).Select(s => s.Id).ToList();

                var ultimoMovimento = (query != null && query.Count > 0) ? query.Max() : 0;


                return await this.ListarDropdownLambda(
                           dropdownInput,
                           senhaMovimentacaoRepository.Object,
                           m => (string.IsNullOrEmpty(dropdownInput.search)
                                 || m.Senha.Numero.ToString().Contains(dropdownInput.search))
                                && (m.TipoLocalChamadaId == localChamada.TipoLocalChamadaId && m.DataHoraFinal == null
                                                                                            && m.Senha.DataHora
                                                                                            > dataDMenos1
                                                                                            && (atendimentoId == 0
                                                                                                && m.Senha.AtendimentoId
                                                                                                == null))
                                || (atendimentoId != 0
                                    && m.Id == ultimoMovimento) //m.Senha.AtendimentoId == atendimentoId)




                           ,
                           p => new DropdownItems
                           {
                               id = p.Id,
                               text = p.Senha.Atendimento != null
                                                   ? p.Senha.Atendimento.Paciente.NomeCompleto
                                                   : p.Senha.Numero.ToString()
                           },
                           o => o.Senha.DataHora.ToString()
                           //,null// o => o.Senha.Id.ToString()
                           //, true
                           //, o => o.Senha.Id
                           //,true
                       ).ConfigureAwait(false);

            }
        }
    }
}
