using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.InternacoesTev;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using Abp.Dependency;

    using SW10.SWMANAGER.Helpers;

    public class TevMovimentoAppService : SWMANAGERAppServiceBase, ITevMovimentoAppService
    {
        private readonly IIocManager _iocManager;

        public TevMovimentoAppService(IIocManager iocManager)
        {
            this._iocManager = iocManager;
        }

        [UnitOfWork]
        public async Task CriarOuEditar(TevMovimentoDto input)
        {
            try
            {
                using (var tevMovimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
                using (var unitOfWorkManager = this._iocManager.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    var tevMovimento = new TevMovimento
                    {
                        AtendimentoId = input.AtendimentoId,
                        Codigo = input.Codigo,
                        CreationTime = input.CreationTime,
                        CreatorUserId = input.CreatorUserId,
                        DeleterUserId = input.DeleterUserId,
                        DeletionTime = input.DeletionTime,
                        Descricao = input.Descricao,
                        Id = input.Id,
                        IsDeleted = input.IsDeleted,
                        IsSistema = input.IsSistema,
                        LastModificationTime = input.LastModificationTime,
                        LastModifierUserId = input.LastModifierUserId,
                        RiscoId = input.RiscoId,
                        Observacao = input.Observacao,
                        Data = input.Data
                    };

                    if (input.Id.Equals(0))
                    {
                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            input.Id = await tevMovimentoRepository.Object.InsertAndGetIdAsync(tevMovimento)
                                           .ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                    else
                    {
                        var entidade = await tevMovimentoRepository.Object.GetAsync(tevMovimento.Id).ConfigureAwait(false);
                        entidade.AtendimentoId = tevMovimento.AtendimentoId;
                        entidade.Codigo = tevMovimento.Codigo;
                        entidade.CreationTime = tevMovimento.CreationTime;
                        entidade.CreatorUserId = tevMovimento.CreatorUserId;
                        entidade.Data = tevMovimento.Data;
                        entidade.DeleterUserId = tevMovimento.DeleterUserId;
                        entidade.DeletionTime = tevMovimento.DeletionTime;
                        entidade.Descricao = tevMovimento.Descricao;
                        //entidade.Id = prescricaoMedica.Id;
                        entidade.Observacao = tevMovimento.Observacao;
                        entidade.RiscoId = tevMovimento.RiscoId;

                        using (var unitOfWork = unitOfWorkManager.Object.Begin())
                        {
                            await tevMovimentoRepository.Object.UpdateAsync(entidade).ConfigureAwait(false);
                            unitOfWork.Complete();
                            unitOfWorkManager.Object.Current.SaveChanges();
                            unitOfWork.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var tevMovimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
                using (var unitOfWorkManager = this._iocManager.ResolveAsDisposable<IUnitOfWorkManager>())
                {
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    {
                        await tevMovimentoRepository.Object.DeleteAsync(id).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<TevMovimentoDto>> ListarTodos()
        {
            try
            {
                using (var tevMovimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
                {
                    var query = tevMovimentoRepository.Object.GetAll().AsNoTracking();

                    var tevMovimentoDto = await query.ToListAsync().ConfigureAwait(false);

                    return new ListResultDto<TevMovimentoDto>
                    {
                        Items = TevMovimentoDto.Mapear(tevMovimentoDto).ToList()
                    };
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<TevMovimentoDto>> Listar(ListarInput input)
        {
            var contar = 0;
            long id = 0;
            long.TryParse(input.PrincipalId, out id);
            try
            {
                using (var tevMovimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
                {
                    var query = tevMovimentoRepository
                        .Object
                        .GetAll()
                        .AsNoTracking()
                        .Include(m => m.Risco)
                        .Include(m => m.Atendimento)
                        .Where(m => m.AtendimentoId == id)
                        .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro) || m.Observacao.Contains(input.Filtro));

                    contar = await query.CountAsync().ConfigureAwait(false);

                    var tevMovimentos = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync().ConfigureAwait(false);

                    var tevMovimentosDto = TevMovimentoDto.Mapear(tevMovimentos).ToList();

                    return new PagedResultDto<TevMovimentoDto>(contar, tevMovimentosDto);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<TevMovimentoDto> Obter(long id)
        {
            try
            {
                using (var tevMovimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
                using (var atendimentoAppService = this._iocManager.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var m = await tevMovimentoRepository.Object.GetAsync(id).ConfigureAwait(false);
                    var ate = new AtendimentoDto();
                    if (m.AtendimentoId.HasValue)
                    {
                        ate = await atendimentoAppService.Object.Obter(m.AtendimentoId.Value).ConfigureAwait(false);
                    }
                    else
                    {
                        ate = null;
                    }

                    var result = TevMovimentoDto.Mapear(m);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<TevMovimentoDto> ObterUltimo(long atendimentoId)
        {
            try
            {
                using (var tevMovimentoRepository =
                    this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
                {
                    var m = await tevMovimentoRepository.Object.GetAll().AsNoTracking().Include(i => i.Risco)
                                .Include(i => i.Atendimento).Where(w => w.AtendimentoId == atendimentoId)
                                .OrderByDescending(o => o.Data).ThenByDescending(o => o.CreationTime)
                                .FirstOrDefaultAsync().ConfigureAwait(false);

                    var result = TevMovimentoDto.Mapear(m);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            using (var tevMovimentoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevMovimento, long>>())
            {
                return await this.CreateSelect2(tevMovimentoRepository.Object).ExecuteAsync(dropdownInput)
                           .ConfigureAwait(false);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarTevRiscoDropdown(DropdownInput dropdownInput)
        {
            using (var tevRiscoRepository = this._iocManager.ResolveAsDisposable<IRepository<TevRisco, long>>())
            {
                return await this.CreateSelect2(tevRiscoRepository.Object).ExecuteAsync(dropdownInput)
                           .ConfigureAwait(false);
            }
        }
    }
}
