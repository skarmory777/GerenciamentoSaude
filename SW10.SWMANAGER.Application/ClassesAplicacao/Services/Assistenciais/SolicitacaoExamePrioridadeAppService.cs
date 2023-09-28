using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;

    public class SolicitacaoExamePrioridadeAppService : SWMANAGERAppServiceBase, ISolicitacaoExamePrioridadeAppService
    {
        private readonly IRepository<SolicitacaoExamePrioridade, long> _solicitacaoExameRepository;
        private readonly IAtendimentoAppService _atendimentoAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;

        public SolicitacaoExamePrioridadeAppService(
            IRepository<SolicitacaoExamePrioridade, long> solicitacaoExameRepository,
            IAtendimentoAppService atendimentoAppService,
            IUnitOfWorkManager unitOfWorkManager,
            IUltimoIdAppService ultimoIdAppService
            )
        {
            _solicitacaoExameRepository = solicitacaoExameRepository;
            _atendimentoAppService = atendimentoAppService;
            _unitOfWorkManager = unitOfWorkManager;
            _ultimoIdAppService = ultimoIdAppService;
        }

        [UnitOfWork]
        public async Task<SolicitacaoExamePrioridadeDto> CriarOuEditar(SolicitacaoExamePrioridadeDto input)
        {
            try
            {
                var solicitacaoExame = SolicitacaoExamePrioridadeDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    var ultimosId = await this._ultimoIdAppService.ListarTodos().ConfigureAwait(false);
                    var ultimoId = ultimosId.Items.FirstOrDefault(m => m.NomeTabela == "SolicitacaoExamePrioridade");
                    solicitacaoExame.Codigo = ultimoId.Codigo;
                    input.Codigo = solicitacaoExame.Codigo;
                    var codigo = Convert.ToInt64(ultimoId.Codigo);
                    codigo++;
                    ultimoId.Codigo = codigo.ToString();
                    await this._ultimoIdAppService.CriarOuEditar(ultimoId).ConfigureAwait(false);
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var _solicitacaoExameId = await this._solicitacaoExameRepository.InsertAndGetIdAsync(solicitacaoExame).ConfigureAwait(false);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                        input.Id = _solicitacaoExameId;
                    }
                }
                else
                {
                    using (var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        var _solicitacaoExame = await this._solicitacaoExameRepository.UpdateAsync(solicitacaoExame).ConfigureAwait(false);
                        unitOfWork.Complete();
                        _unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                        input = SolicitacaoExamePrioridadeDto.Mapear(_solicitacaoExame);
                    }
                }
                return input;
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
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    await this._solicitacaoExameRepository.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        /// <inheritdoc />
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<SolicitacaoExamePrioridadeDto>> ListarTodos()
        {
            try
            {
                var query = this._solicitacaoExameRepository
                    .GetAll().AsNoTracking();


                var admissoesMedicasDto = await query.ToListAsync().ConfigureAwait(false);

                return new ListResultDto<SolicitacaoExamePrioridadeDto> { Items = SolicitacaoExamePrioridadeDto.Mapear(admissoesMedicasDto).ToList() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<SolicitacaoExamePrioridadeDto>> Listar(ListarInput input)
        {
            var admissoesMedicasDto = new List<SolicitacaoExamePrioridadeDto>();
            try
            {
                var query = _solicitacaoExameRepository
                    .GetAll()
                    .AsNoTracking()
                    .Where(m => m.CreationTime >= input.StartDate && m.CreationTime <= input.EndDate)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m => m.Codigo.Contains(input.Filtro) || m.Descricao.Contains(input.Filtro))
                    .OrderBy(input.Sorting)
                    .PageBy(input);

                var contarAdmissoesMedicas = await query.CountAsync().ConfigureAwait(false);
                admissoesMedicasDto = SolicitacaoExamePrioridadeDto.Mapear(await query.ToListAsync().ConfigureAwait(false)).ToList();

                return new PagedResultDto<SolicitacaoExamePrioridadeDto>(contarAdmissoesMedicas, admissoesMedicasDto);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                var query = this._solicitacaoExameRepository.GetAll()
                    .AsNoTracking()
                    .WhereIf(
                        !dropdownInput.search.IsNullOrEmpty(),
                        m => m.Codigo.Contains(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search))
                    .OrderBy(p => p.Descricao)
                    .Select(p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) });

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<SolicitacaoExamePrioridadeDto> Obter(long id)
        {
            try
            {
                var query = await this._solicitacaoExameRepository
                                .GetAll()
                                .AsNoTracking()
                                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);

                var result = SolicitacaoExamePrioridadeDto.Mapear(query);
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<SolicitacaoExamePrioridadeDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = _solicitacaoExameRepository
                    .GetAll()
                    .AsNoTracking()
                    .WhereIf(!filtro.IsNullOrWhiteSpace(), m => m.Codigo.Contains(filtro) || m.Descricao.Contains(filtro));

                var admissoesMedicas = await query.ToListAsync().ConfigureAwait(false);
                var admissoesMedicasDto = SolicitacaoExamePrioridadeDto.Mapear(admissoesMedicas).ToList();

                return new ListResultDto<SolicitacaoExamePrioridadeDto> { Items = admissoesMedicasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}
