using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Diagnostico.Imagens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais
{
    using Abp.Auditing;
    using SW10.SWMANAGER.Helpers;
    using System.Text;

    public class SolicitacaoExameItemAppService : SWMANAGERAppServiceBase, ISolicitacaoExameItemAppService
    {
        private readonly IRepository<SolicitacaoExameItem, long> _solicitacaoExameItemRepository;
        private readonly IRepository<LaudoMovimentoItem, long> _laudoMovimentoItemRepository;
        private readonly IAtendimentoAppService _atendimentoAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUltimoIdAppService _ultimoIdAppService;
        private readonly IFaturamentoItemAppService _faturamentoItemAppService;
        private readonly IMaterialAppService _materialItemAppService;
        private readonly IKitExameAppService _kitExameAppService;
        private readonly ISolicitacaoExameAppService _solicitacaoExameAppService;
        private readonly IRepository<SolicitacaoExame, long> _solicitacaoExameRepository;


        public SolicitacaoExameItemAppService(
            IRepository<SolicitacaoExameItem, long> solicitacaoExameItemRepository,
            IRepository<LaudoMovimentoItem, long> laudoMovimentoItemRepository,
            IAtendimentoAppService atendimentoAppService,
            IUnitOfWorkManager unitOfWorkManager,
            IUltimoIdAppService ultimoIdAppService,
            IFaturamentoItemAppService faturamentoItemAppService,
            IMaterialAppService materialAppService,
            IKitExameAppService kitExameAppService,
            ISolicitacaoExameAppService solicitacaoExameAppService,
            IRepository<SolicitacaoExame, long> solicitacaoExameRepository
            )
        {
            this._solicitacaoExameItemRepository = solicitacaoExameItemRepository;
            this._laudoMovimentoItemRepository = laudoMovimentoItemRepository;
            this._atendimentoAppService = atendimentoAppService;
            this._unitOfWorkManager = unitOfWorkManager;
            this._ultimoIdAppService = ultimoIdAppService;
            this._faturamentoItemAppService = faturamentoItemAppService;
            this._materialItemAppService = materialAppService;
            this._kitExameAppService = kitExameAppService;
            this._solicitacaoExameAppService = solicitacaoExameAppService;
            this._solicitacaoExameRepository = solicitacaoExameRepository;
        }

        [UnitOfWork]
        public async Task<SolicitacaoExameItemDto> CriarOuEditar(SolicitacaoExameItemDto input)
        {
            try
            {
                var solicitacaoExameItem = new SolicitacaoExameItem
                {
                    Codigo = input.Codigo,
                    CreationTime = input.CreationTime,
                    CreatorUserId = input.CreatorUserId,
                    DataValidade = input.DataValidade,
                    DeleterUserId = input.DeleterUserId,
                    DeletionTime = input.DeletionTime,
                    Descricao = input.Descricao,
                    FaturamentoItemId = input.FaturamentoItemId,
                    GuiaNumero = input.GuiaNumero,
                    Id = input.Id,
                    IsDeleted = input.IsDeleted,
                    IsSistema = input.IsSistema,
                    Justificativa = input.Justificativa,
                    KitExameId = input.KitExameId,
                    LastModificationTime = input.LastModificationTime,
                    LastModifierUserId = input.LastModifierUserId,
                    MaterialId = input.MaterialId,
                    SenhaNumero = input.SenhaNumero,
                    SolicitacaoExameId = input.SolicitacaoExameId
                };
                if (input.Id.Equals(0))
                {
                    // var ultimosId = await _ultimoIdAppService.ListarTodos();
                    // var ultimoId = ultimosId.Items.Where(m => m.NomeTabela == "SolicitacaoExameItem").FirstOrDefault();
                    // solicitacaoExameItem.Codigo = ultimoId.Codigo;
                    // input.Codigo = solicitacaoExameItem.Codigo;
                    // var codigo = Convert.ToInt64(ultimoId.Codigo);
                    // codigo++;
                    // ultimoId.Codigo = codigo.ToString();
                    // await _ultimoIdAppService.CriarOuEditar(ultimoId);
                    using (var unitOfWork = this._unitOfWorkManager.Begin())
                    {
                        input.Id = await this._solicitacaoExameItemRepository.InsertAndGetIdAsync(solicitacaoExameItem).ConfigureAwait(false);
                        unitOfWork.Complete();
                        this._unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
                else
                {
                    using (var unitOfWork = this._unitOfWorkManager.Begin())
                    {
                        await this._solicitacaoExameItemRepository.UpdateAsync(solicitacaoExameItem).ConfigureAwait(false);
                        unitOfWork.Complete();
                        this._unitOfWorkManager.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }

                return input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroSalvar"), ex);
            }
        }

        [UnitOfWork]
        public async Task Excluir(long id)
        {
            try
            {
                using (var unitOfWork = this._unitOfWorkManager.Begin())
                {
                    await this._solicitacaoExameItemRepository.DeleteAsync(id).ConfigureAwait(false);
                    unitOfWork.Complete();
                    this._unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroExcluir"), ex);
            }

        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<SolicitacaoExameItemDto>> ListarTodos()
        {
            try
            {
                var query = this._solicitacaoExameItemRepository.GetAll().AsNoTracking().Include(m => m.FaturamentoItem)

                        // .Include(m => m.KitExame)
                        .Include(m => m.Material)

                    // .Include(m => m.Solicitacao);
                    ;

                var admissoesMedicasDto = await query.ToListAsync().ConfigureAwait(false);

                return new ListResultDto<SolicitacaoExameItemDto> { Items = SolicitacaoExameItemDto.Mapear(admissoesMedicasDto).ToList() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<SolicitacaoExameItemList>> Listar(ListarSolicitacaoExameItensInput input)
        {
            var contarSolicitacaoExameItens = 0;
            var admissoesMedicasDto = new List<SolicitacaoExameItemList>();
            try
            {
                var query = this._solicitacaoExameItemRepository.GetAll().AsNoTracking().Include(m => m.FaturamentoItem)

                    // .Include(m => m.KitExame)
                    .Include(m => m.Material)

                    // .Include(m => m.Solicitacao)
                    .Where(m => m.SolicitacaoExameId == input.SolicitacaoExameId)
                    .WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m =>
                            m.Material.Descricao.Contains(input.Filtro)
                            || m.Material.Codigo.Contains(input.Filtro)
                            || m.Solicitacao.Codigo.Contains(input.Filtro)
                            || m.Solicitacao.MedicoSolicitante.NomeCompleto
                                .Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente
                                .NomeCompleto.Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.Cpf
                                .Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.Nascimento
                                .ToString().Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.Rg
                                .Contains(input.Filtro))
                    .OrderBy(input.Sorting)
                    .PageBy(input);
                contarSolicitacaoExameItens = await query.CountAsync().ConfigureAwait(false);
                var solicitacoesExameItem = await query.Select(m => new SolicitacaoExameItemList
                {
                    FaturamentoItem = m.FaturamentoItem.Descricao,
                    GuiaNumero = m.GuiaNumero,
                    Id = m.Id,
                    IsDeleted = m.IsDeleted,
                    IsSistema = m.IsSistema,
                    Material = m.Material.Descricao
                }).ToListAsync().ConfigureAwait(false);
                return new PagedResultDto<SolicitacaoExameItemList>(contarSolicitacaoExameItens, solicitacoesExameItem);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<SolicitacaoExameItemList>> ListarAtendimento(ListarSolicitacaoExameItensInput input)
        {
            // input.Id = id.ToString();
            // input.MaxResultCount = 50;
            var contarSolicitacaoExameItens = 0;
            var admissoesMedicasDto = new List<SolicitacaoExameItemList>();
            try
            {
                long atendimentoId = 0;

                var boolAte = long.TryParse(input.Id, out atendimentoId);
                var query = this._solicitacaoExameItemRepository.GetAll().Include(m => m.FaturamentoItem)
                    .Include(m => m.FaturamentoItem.Grupo)

                    // .Include(m => m.KitExame)
                    .Include(m => m.Material).Include(m => m.Solicitacao).Where(
                        w => w.Solicitacao.AtendimentoId == atendimentoId
                             && (w.StatusSolicitacaoExameItemId == 1 || w.StatusSolicitacaoExameItemId == 3
                                                                     || w.StatusSolicitacaoExameItemId == null)
                             && (w.FaturamentoItem.IsLaboratorio || w.FaturamentoItem.Grupo.IsLaboratorio))

                    // .WhereIf(atendimentoId > 0, m => m.Solicitacao.AtendimentoId == atendimentoId)
                    .WhereIf(
                        !input.Filtro.IsNullOrEmpty(),
                        m =>
                            m.Material.Descricao.Contains(input.Filtro)
                            || m.Material.Codigo.Contains(input.Filtro)
                            || m.Solicitacao.Codigo.Contains(input.Filtro)
                            || m.Solicitacao.MedicoSolicitante.NomeCompleto.Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.CodigoPaciente.ToString()
                                .Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.NomeCompleto
                                .Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.Cpf.Contains(input.Filtro)
                            || m.Solicitacao.Atendimento.Paciente.Nascimento.ToString()
                                .Contains(input.Filtro) || m.Solicitacao.Atendimento.Paciente.Rg
                                .Contains(input.Filtro));

                contarSolicitacaoExameItens = await query.CountAsync().ConfigureAwait(false);

                var solicitacaoList = await query

                                          // .AsQueryable()
                                          // .AsNoTracking()
                                          .OrderBy(input.Sorting)
                                          .PageBy(input)
                                          .ToListAsync().ConfigureAwait(false);

                var solicitacoesExameItem = solicitacaoList.Select(m => new SolicitacaoExameItemList
                {
                    FaturamentoItem = m.FaturamentoItem?.Descricao,
                    GuiaNumero = m.GuiaNumero,
                    Id = m.Id,
                    IsDeleted = m.IsDeleted,
                    IsSistema = m.IsSistema,
                    Material = m.Material?.Descricao
                }).ToList();
                return new PagedResultDto<SolicitacaoExameItemList>(contarSolicitacaoExameItens, solicitacoesExameItem);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<SolicitacaoExameItemDto> Obter(long id)
        {
            try
            {
                var query = await this._solicitacaoExameItemRepository.GetAll().Where(m => m.Id == id)

                                // .Include(m => m.FaturamentoItem)
                                // .Include(m => m.KitExame)
                                // .Include(m => m.Material)
                                // .Include(m => m.Solicitacao)
                                // .Include(m => m.Solicitacao.Atendimento)
                                // .Include(m => m.Solicitacao.Atendimento.Paciente)
                                .Select(m => new SolicitacaoExameItemDto
                                {
                                    Codigo = m.Codigo,
                                    CreationTime = m.CreationTime,
                                    CreatorUserId = m.CreatorUserId,
                                    DataValidade = m.DataValidade,
                                    DeleterUserId = m.DeleterUserId,
                                    DeletionTime = m.DeletionTime,
                                    Descricao = m.Descricao,
                                    FaturamentoItemId = m.FaturamentoItemId,
                                    GuiaNumero = m.GuiaNumero,
                                    Id = m.Id,
                                    IsDeleted = m.IsDeleted,
                                    IsSistema = m.IsSistema,
                                    Justificativa = m.Justificativa,
                                    KitExameId = m.KitExameId,
                                    LastModificationTime = m.LastModificationTime,
                                    LastModifierUserId = m.LastModifierUserId,
                                    MaterialId = m.MaterialId,
                                    SenhaNumero = m.SenhaNumero,
                                    SolicitacaoExameId = m.SolicitacaoExameId
                                })
                                .FirstOrDefaultAsync().ConfigureAwait(false);

                var result = query;


                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<List<SolicitacaoExameItemDto>> ObterPorLista(List<long> ids)
        {
            try
            {
                var result = (await this._solicitacaoExameItemRepository
                                 .GetAll()
                                 .AsNoTracking()
                                 .Where(m => ids.Any(a => a == m.Id))
                                 .ToListAsync().ConfigureAwait(false)).Select(item => SolicitacaoExameItemDto.Mapear(item)).ToList();


                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        if (item.SolicitacaoExameId != 0)
                        {
                            var solicitacao = this._solicitacaoExameRepository
                                                                         .GetAll().
                                                                         FirstOrDefault(w => w.Id == item.SolicitacaoExameId);
                            item.Solicitacao = new SolicitacaoExameDto();

                            if (solicitacao.AtendimentoId != null)
                            {
                                item.Solicitacao.Atendimento = await this._atendimentoAppService.Obter((long)solicitacao.AtendimentoId).ConfigureAwait(false);
                            }
                        }

                        if (item.FaturamentoItemId != null)
                        {
                            item.FaturamentoItem = await this._faturamentoItemAppService.Obter((long)item.FaturamentoItemId).ConfigureAwait(false);
                        }


                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [UnitOfWork(false)]
        public async Task<SolicitacaoExameItemDto> ObterParaEdicao(long id)
        {
            try
            {
                var query = await this._solicitacaoExameItemRepository
                                .GetAll()
                                .Include(m => m.FaturamentoItem)
                                .Include(m => m.Material)
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync().ConfigureAwait(false);

                var result = SolicitacaoExameItemDto.Mapear(query);
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<SolicitacaoExameItemDto>> ListarFiltro(string filtro)
        {
            try
            {
                var query = this._solicitacaoExameItemRepository
                    .GetAll()
                    .AsNoTracking()
                    .Include(m => m.FaturamentoItem)
                    .Include(m => m.KitExame)
                    .Include(m => m.Material)
                    .Include(m => m.Solicitacao)
                    .WhereIf(!filtro.IsNullOrWhiteSpace(), m =>
                         m.CreationTime.ToShortTimeString().Contains(filtro) ||
                         m.Solicitacao.Atendimento.Medico.NomeCompleto.Contains(filtro) ||
                         m.Solicitacao.Atendimento.Medico.Cpf.Contains(filtro) ||
                         m.Solicitacao.Atendimento.Medico.Nascimento.Value.ToShortTimeString().Contains(filtro) ||
                         m.Solicitacao.Atendimento.Medico.Rg.Contains(filtro) ||
                         m.Solicitacao.Atendimento.Paciente.NomeCompleto.Contains(filtro) ||
                         m.Solicitacao.Atendimento.Paciente.Cpf.Contains(filtro) ||
                         ((DateTime)m.Solicitacao.Atendimento.Paciente.Nascimento).ToShortTimeString().Contains(filtro) || m
                             .Solicitacao.Atendimento.Paciente.Rg.Contains(filtro));

                var admissoesMedicas = await query.ToListAsync().ConfigureAwait(false);
                var admissoesMedicasDto = SolicitacaoExameItemDto.Mapear(admissoesMedicas).ToList();

                return new ListResultDto<SolicitacaoExameItemDto> { Items = admissoesMedicasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<SolicitacaoExameItemList>> ListarPorSolicitacao(ListarSolicitacaoExameItensInput input)
        {
            try
            {
                var id = Convert.ToInt64(input.Filtro);
                var query = this._solicitacaoExameItemRepository.GetAll().Include(m => m.FaturamentoItem)

                    // .Include(m => m.KitExame)
                    .Include(m => m.Material)

                    // .Include(m => m.Solicitacao)
                    .Where(m => m.SolicitacaoExameId == id);

                var count = await query.CountAsync().ConfigureAwait(false);
                var listDto = await query.Select(m => new SolicitacaoExameItemList
                {
                    FaturamentoItem = m.FaturamentoItem.Descricao,
                    GuiaNumero = m.GuiaNumero,
                    Id = m.Id,
                    IsDeleted = m.IsDeleted,
                    IsSistema = m.IsSistema,
                    Material = m.Material.Descricao,
                    AccessNumber = m.AccessNumber
                }).ToListAsync().ConfigureAwait(false);

                listDto.ForEach(x => x.AccessNumber = FormatAccessNumber(x.AccessNumber));

                return new PagedResultDto<SolicitacaoExameItemList>
                {
                    TotalCount = count,
                    Items = listDto
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        public static string FormatAccessNumber(string accessNumber)
        {
            if (string.IsNullOrEmpty(accessNumber))
            {
                return accessNumber;
            }

            return "radiant://?n=paet&v=SERVER&n=pstv&v=00080050&v=" + accessNumber;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<SolicitacaoExameItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var solicitacaoExameItens = this._solicitacaoExameItemRepository.GetAll().AsNoTracking()

                    .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            )
                            .Include(s => s.Solicitacao)
                            .Include(a => a.Solicitacao.Atendimento)
                            .ToList()
                            ;

                var laudoMovs = this._laudoMovimentoItemRepository.GetAll().AsNoTracking()
                    .Include(s => s.SolicitacaoExameItem.Solicitacao.Atendimento)

                    // .Where(l=>l.SolicitacaoExameItem.Solicitacao.AtendimentoId.Equals(dropdownInput.filtro))
                    .ToList();


                var naoPertencentes = new List<SolicitacaoExameItem>();
                for (var i = 0; i < solicitacaoExameItens.Count(); i++)
                {
                    var removido = false;
                    if (solicitacaoExameItens[i].Solicitacao.Atendimento.Id.ToString() != dropdownInput.filtro)
                    {
                        naoPertencentes.Add(solicitacaoExameItens[i]);
                        removido = true;
                    }

                    foreach (var laudoMovItem in laudoMovs)
                    {
                        if (laudoMovItem.SolicitacaoExameItem != null && laudoMovItem.SolicitacaoExameItem == solicitacaoExameItens[i] && !removido)
                        {
                            naoPertencentes.Add(solicitacaoExameItens[i]);
                        }
                    }
                }

                foreach (var sei in naoPertencentes)
                {
                    solicitacaoExameItens.Remove(sei);
                }

                var seiIds = solicitacaoExameItens.Select(s => s.Id);

                var query = from p in this._solicitacaoExameItemRepository.GetAll().AsNoTracking()
                        .Where(t => seiIds.Contains(t.Id))

                                // var query = (IQueryable<DropdownItems>)
                                // from p in solicitacaoExameItens
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync().ConfigureAwait(false);

                var total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownNaoRegistradoPorAtendimento(DropdownInput dropdownInput)
        {
            long atendimentoId;
            DateTime dataHoraRegistro;

            long.TryParse(dropdownInput.filtros[0], out atendimentoId);
            DateTime.TryParse(dropdownInput.filtros[1], out dataHoraRegistro);

            var dataHoraMinimaSolicitacaoExame = dataHoraRegistro.AddHours(-48);


            return await base.ListarDropdownLambda(dropdownInput
                       , this._solicitacaoExameItemRepository
                       , w => (w.Solicitacao.AtendimentoId == atendimentoId
                               && w.Solicitacao.DataSolicitacao >= dataHoraMinimaSolicitacaoExame
                               && (w.StatusSolicitacaoExameItemId == (long)EnumSolicitacaoExameItem.Liberado || w.StatusSolicitacaoExameItemId == null)
                               && (w.FaturamentoItem.IsLaudo || w.FaturamentoItem.Grupo.IsLaudo)
                           )
                       , p => new DropdownItems { id = p.FaturamentoItem.Id, text = string.Concat(p.FaturamentoItem.Codigo.ToString(), " - ", p.FaturamentoItem.Descricao) }
                       , o => o.FaturamentoItem.Codigo).ConfigureAwait(false);
        }



        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownExameLaboratorioNaoRegistradoPorAtendimento(DropdownInput dropdownInput)
        {
            long atendimentoId;

            long.TryParse(dropdownInput.filtros[0], out atendimentoId);

            // DateTime.TryParse(dropdownInput.filtros[1], out dataHoraRegistro);

            // var dataHoraMinimaSolicitacaoExame = dataHoraRegistro.AddHours(-48);
            return await base.ListarDropdownLambda(
                       dropdownInput,
                       this._solicitacaoExameItemRepository,
                       w => (w.Solicitacao.AtendimentoId == atendimentoId

                             // && w.Solicitacao.DataSolicitacao >= dataHoraMinimaSolicitacaoExame
                             && (w.StatusSolicitacaoExameItemId == (long)EnumSolicitacaoExameItem.Liberado
                                 || w.StatusSolicitacaoExameItemId == null) && (w.FaturamentoItem.IsLaboratorio || w.FaturamentoItem.Grupo.IsLaboratorio)),
                       p => new DropdownItems
                       {
                           id = p.FaturamentoItem.Id,
                           text = string.Concat(
                                        p.FaturamentoItem.Codigo.ToString(),
                                        " - ",
                                        p.FaturamentoItem.Descricao)
                       },
                       o => o.FaturamentoItem.Codigo).ConfigureAwait(false);
        }

        [UnitOfWork(false)]
        public async Task<PagedResultDto<RegistroExameIndex>> ListarExamesImagensNaoRegistrados(ListarExameSolicitadosInput input)
        {
            try
            {
                if (input.Sorting.Equals("Paciente.NomeCompleto"))
                    input.Sorting = "Solicitacao.Atendimento.Paciente.NomeCompleto";

                var query = this._solicitacaoExameItemRepository.GetAll().AsNoTracking()
                                                                          .Include(i => i.FaturamentoItem)
                                                                          .Include(i => i.Solicitacao.Atendimento.Paciente)
                                                                          .Include(i => i.Solicitacao.Atendimento.Paciente.SisPessoa)
                                                                          .Include(i => i.Solicitacao.Atendimento.Convenio)
                                                                          .Include(i => i.Solicitacao.Atendimento.Convenio.SisPessoa)
                                                                          .Where(w => (w.StatusSolicitacaoExameItemId == (long)EnumSolicitacaoExameItem.Liberado || w.StatusSolicitacaoExameItemId == null)
                                                                                   && (w.FaturamentoItem.IsLaudo || w.FaturamentoItem.Grupo.IsLaudo)
                                                                                   && (input.AtendimentoId == null || w.Solicitacao.AtendimentoId == input.AtendimentoId)
                                                                                   && (input.ConvenioId == null || w.Solicitacao.Atendimento.ConvenioId == input.ConvenioId)
                                                                                   && (input.PacienteId == null || w.Solicitacao.Atendimento.PacienteId == input.PacienteId)
                                                                                   && ((input.EmissaoDe == null || input.EmissaoAte == null) || input.EmissaoDe <= w.Solicitacao.DataSolicitacao && w.Solicitacao.DataSolicitacao <= input.EmissaoAte)
                                                                        );

                var data = await query.OrderBy(input.Sorting).PageBy(input).Select(item => new RegistroExameIndex()
                {
                    Id = item.Id,
                    Exame = item.FaturamentoItem != null ? item.FaturamentoItem.Descricao : "",
                    PacienteDescricao = item.Solicitacao != null && item.Solicitacao.Atendimento != null && item.Solicitacao.Atendimento.Paciente != null ? item.Solicitacao.Atendimento.Paciente.NomeCompleto : "",
                    ConvenioDescricao = item.Solicitacao != null && item.Solicitacao.Atendimento != null && item.Solicitacao.Atendimento.Convenio != null && item.Solicitacao.Atendimento.Convenio.NomeFantasia != null ? item.Solicitacao.Atendimento.Convenio.NomeFantasia : "",
                    AtendimentoId = item.Solicitacao != null ? item.Solicitacao.AtendimentoId : 0,
                    Data = item.Solicitacao != null ? item.Solicitacao.DataSolicitacao : DateTime.MinValue,
                    AccessNumber = item.AccessNumber
                }).ToListAsync();

                data.ForEach(x => x.AccessNumber = FormatAccessNumber(x.AccessNumber));

                return new PagedResultDto<RegistroExameIndex>(await query.CountAsync(), data);
            }
            catch (Exception e)
            {

            }
            return null;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<RegistroExameIndex>> ListarExamesLaboratoriaisNaoColetados(ListarExameSolicitadosInput input)
        {
            try
            {
                var dataTableQuery = this.CreateDataTable<RegistroExameIndex, ListarExameSolicitadosInput>();

                return await dataTableQuery
                    .AddDefaultField("Exame.Id")
                    .AddSelectClause(@"
                            Exame.Id,
                            Exame.Codigo,
                            Paciente.NomeCompleto AS PacienteDescricao,
                            Convenio.NomeFantasia AS ConvenioDescricao,
                            Atendimento.Id AS AtendimentoId,
                            Exame.DataSolicitacao AS DataSolicitacao,
                            Atendimento.DataRegistro AS DataAtendimento,
                            MedicoPessoa.NomeCompleto AS Medico,
                            Atendimento.Codigo AS Codigo,
                            Leito.Descricao AS Leito,
                            TipoAcomodacao.Descricao AS TipoLeito,
                            (CASE WHEN Atendimento.IsInternacao = 1 THEN
                            'Internação'
                            ELSE 
                            'Ambulatório/Emergêcia'
                            END) AS TipoAtendimento")
                    .AddFromClause($@"
                        AssSolicitacaoExame Exame
                        LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Exame.AtendimentoId
                        LEFT JOIN SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id
                        LEFT JOIN SisMedico AS Medico ON Exame.SisMedicoSolicitanteId = Medico.Id
                        LEFT JOIN SisPessoa AS MedicoPessoa ON Medico.SisPessoaId = MedicoPessoa.Id
                        LEFT JOIN SisConvenio AS Convenio ON Atendimento.SisConveniolId = Convenio.Id
                        LEFT JOIN AteLeito AS Leito ON Atendimento.AteLeitoId = Leito.Id
                        LEFT JOIN SisTipoAcomodacao AS TipoAcomodacao ON Leito.SisTipoAcomodacaoId = TipoAcomodacao.Id
                        INNER JOIN
                        (
                        SELECT
                            DISTINCT
                            Atendimento.Id AS AtendimentoId,
                            Max(Exame.Id) AS ExameId
                        FROM 
                            AssSolicitacaoExameItem ExameItem
                            LEFT JOIN  AssSolicitacaoExame Exame ON Exame.Id = ExameItem.AssSolicitacaoExameId AND Exame.IsDeleted = @deleted
                            LEFT JOIN FatItem ON FatItem.Id = ExameItem.FatItemId AND FatItem.IsDeleted = @deleted
                            LEFT JOIN FatGrupo ON FatGrupo.Id = FatItem.GrupoId AND FatGrupo.IsDeleted = @deleted
                            LEFT JOIN AteAtendimento AS Atendimento ON Atendimento.Id = Exame.AtendimentoId AND Atendimento.IsDeleted = @deleted
                            LEFT JOIN SisPaciente AS Paciente ON Atendimento.SisPacienteId = Paciente.Id  AND Paciente.IsDeleted = @deleted
                            LEFT JOIN SisMedico AS Medico ON Exame.SisMedicoSolicitanteId = Medico.Id AND Medico.IsDeleted = @deleted
                            LEFT JOIN SisPessoa AS MedicoPessoa ON Medico.SisPessoaId = MedicoPessoa.Id AND MedicoPessoa.IsDeleted = @deleted
                            LEFT JOIN SisConvenio AS Convenio ON Atendimento.SisConveniolId = Convenio.Id AND Convenio.IsDeleted = @deleted
                            LEFT JOIN AteLeito AS Leito ON Atendimento.AteLeitoId = Leito.Id AND Leito.IsDeleted = @deleted
                            LEFT JOIN SisTipoAcomodacao AS TipoAcomodacao ON Leito.SisTipoAcomodacaoId = TipoAcomodacao.Id AND TipoAcomodacao.IsDeleted = @deleted
                            WHERE (ExameItem.StatusSolicitacaoExameItemId = {(long)EnumSolicitacaoExameItem.Liberado} OR ExameItem.StatusSolicitacaoExameItemId is null)
                            AND(FatItem.IsLaboratorio = 1 OR FatGrupo.IsLaboratorio = 1)
                            GROUP BY Atendimento.Id
                            ) AS InnerExame ON
                            InnerExame.AtendimentoId = Exame.AtendimentoId
                            AND InnerExame.ExameId = Exame.Id AND Exame.IsDeleted = @deleted")
                    .AddWhereMethod((filtro, dapperParameters) =>
                    {
                        dapperParameters.Add("deleted", false);
                        dapperParameters.Add("isSistema", false);
                        var where = new StringBuilder("AND 1 = 1 ")
                            .WhereIf(filtro.EmpresaId.HasValue, " AND Atendimento.SisEmpresaId = @EmpresaId")
                            .WhereIf(filtro.EndDate.HasValue && filtro.StartDate.HasValue, " AND DataSolicitacao BETWEEN @StartDate AND @EndDate")
                            .WhereIf(filtro.MedicoId.HasValue, " AND Medico.Id = @MedicoId")
                            .WhereIf(filtro.ConvenioId.HasValue, " AND Convenio.Id = @ConvenioId")
                            .WhereIf(filtro.UnidadeId.HasValue, " AND Atendimento.SisUnidadeOrganizacionalId = @UnidadeId")
                            .WhereIf(filtro.PacienteId.HasValue, " AND Paciente.Id = @PacienteId")
                            .WhereIf(filtro.TipoAtendimento == "INT", " AND Atendimento.IsInternacao = 1")
                            .WhereIf(filtro.TipoAtendimento == "AMB", " AND Atendimento.IsAmbulatorioEmergencia = 1");
                        return where.ToString();
                    }).ExecuteAsync(input);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(this.L("ErroPesquisar"), ex);
            }
        }
    }
}
