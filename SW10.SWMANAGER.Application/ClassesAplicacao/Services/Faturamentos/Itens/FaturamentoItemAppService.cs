using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens
{
    using Abp.Auditing;
    using Abp.Dependency;
    using Castle.Core.Internal;
    using SW10.SWMANAGER.Helper;
    using SW10.SWMANAGER.Helpers;
    using System.Text;
    using System.Web.Razor.Parser;

    public class FaturamentoItemAppService : SWMANAGERAppServiceBase, IFaturamentoItemAppService
    {
        #region Dependencias
        private readonly IRepository<FaturamentoItem, long> _itemRepository;
        private readonly IListarItensExcelExporter _listarItensExcelExporter;
        private readonly IFaturamentoGrupoAppService _faturamentoGrupoAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public FaturamentoItemAppService(
            IRepository<FaturamentoItem, long> itemRepository,
            IListarItensExcelExporter listarItensExcelExporter,
            IFaturamentoGrupoAppService faturamentoGrupoAppService,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _itemRepository = itemRepository;
            _listarItensExcelExporter = listarItensExcelExporter;
            _faturamentoGrupoAppService = faturamentoGrupoAppService;
            _unitOfWorkManager = unitOfWorkManager;
        }
        #endregion dependencias.

        public async Task<PagedResultDto<FaturamentoItemDto>> Listar(ListarFaturamentoItensInput input)
        {
            var itemrItens = 0;
            List<FaturamentoItem> itens;
            var itensDtos = new List<FaturamentoItemDto>();

            try
            {
                var query = _itemRepository
                    .GetAll()
                    .Include(i => i.Grupo)
                    .Include(i => i.SubGrupo)
                    .Include(i => i.Grupo.TipoGrupo)
                    .WhereIf(input.GrupoId != 0, m => input.GrupoId == m.Grupo.Id)
                    .WhereIf(input.SubGrupoId != 0, m => input.SubGrupoId == m.SubGrupo.Id)
                    .WhereIf(input.TipoId != 0, x => x.Grupo.TipoGrupoId == input.TipoId)

                    .WhereIf(!input.Filtro.IsNullOrEmpty(),
                        m => m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                        || m.Grupo.Descricao.Contains(input.Filtro.ToUpper())
                        || m.SubGrupo.Descricao.Contains(input.Filtro.ToUpper())
                        || m.Grupo.TipoGrupo.Descricao.Contains(input.Filtro.ToUpper())
                        || m.Grupo.Descricao.Contains(input.Grupo.ToUpper())
                        || m.SubGrupo.Descricao.Contains(input.SubGrupo.ToUpper())
                        || m.Grupo.TipoGrupo.Descricao.Contains(input.Tipo.ToUpper())
                        )
                    ;

                itemrItens = await query.CountAsync().ConfigureAwait(false);

                itens = await query
                            .AsNoTracking()
                            .OrderBy(input.Sorting)
                            .PageBy(input)
                            .ToListAsync().ConfigureAwait(false);

                itensDtos = FaturamentoItemDto.Mapear(itens).ToList();

                return new PagedResultDto<FaturamentoItemDto>(
                    itemrItens,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task CriarOuEditar(FaturamentoItemDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                {
                    var Item = FaturamentoItemDto.Mapear(input);

                    if (Item.LaudoGrupoId == 0)
                        Item.LaudoGrupoId = null;

                    if (input.Id.Equals(0))
                    {
                        await this._itemRepository.InsertAsync(Item).ConfigureAwait(false);
                    }
                    else
                    {
                        var itemEntity = _itemRepository.GetAll()
                                                        .Where(w => w.Id == input.Id)
                                                        .FirstOrDefault();

                        if (itemEntity != null)
                        {
                            itemEntity.Codigo = input.Codigo;
                            itemEntity.Descricao = input.Descricao;
                            itemEntity.BrasItemId = input.BrasItemId;
                            itemEntity.GrupoId = input.GrupoId != 0 ? input.GrupoId : null;
                            itemEntity.SubGrupoId = input.SubGrupoId != 0 ? input.SubGrupoId : null;
                            itemEntity.LaudoGrupoId = input.LaudoGrupoId != 0 ? input.LaudoGrupoId : null;
                            itemEntity.DescricaoTuss = input.DescricaoTuss;
                            itemEntity.Observacao = input.Observacao;
                            itemEntity.CodAmb = input.CodAmb;
                            itemEntity.CodTuss = input.CodTuss;
                            itemEntity.CodCbhpm = input.CodCbhpm;
                            itemEntity.DivideBrasindice = input.DivideBrasindice;
                            itemEntity.Referencia = input.Referencia;
                            itemEntity.ReferenciaSihSus = input.ReferenciaSihSus;
                            itemEntity.Sexo = input.Sexo;
                            itemEntity.QtdLaudo = input.QtdLaudo;
                            itemEntity.TipoLaudo = input.TipoLaudo;
                            itemEntity.DuracaoMinima = input.DuracaoMinima;
                            itemEntity.IsAtivo = input.IsAtivo;
                            itemEntity.IsObrigaMedico = input.IsObrigaMedico;
                            itemEntity.IsTaxaUrgencia = input.IsTaxaUrgencia;
                            itemEntity.IsPediatria = input.IsPediatria;
                            itemEntity.IsProcedimentoSerie = input.IsProcedimentoSerie;
                            itemEntity.IsRequisicaoExame = input.IsRequisicaoExame;
                            itemEntity.IsPermiteRevisao = input.IsPermiteRevisao;
                            itemEntity.IsPrecoManual = input.IsPrecoManual;
                            itemEntity.IsAutorizacao = input.IsAutorizacao;
                            itemEntity.IsInternacao = input.IsInternacao;
                            itemEntity.IsAmbulatorio = input.IsAmbulatorio;
                            itemEntity.IsCirurgia = input.IsCirurgia;
                            itemEntity.IsPorte = input.IsPorte;
                            itemEntity.IsConsultor = input.IsConsultor;
                            itemEntity.IsLaboratorio = input.IsLaboratorio;
                            itemEntity.IsPlantonista = input.IsPlantonista;
                            itemEntity.IsOpme = input.IsOpme;
                            itemEntity.IsExtraCaixa = input.IsExtraCaixa;
                            itemEntity.IsLaudo = input.IsLaudo;

                            itemEntity.IsAgendaCirurgia = input.IsAgendaCirurgia;
                            itemEntity.IsAgendaConsulta = input.IsAgendaConsulta;
                            itemEntity.IsAgendaExame = input.IsAgendaExame;
                            itemEntity.QuantidadeMinutos = input.QuantidadeMinutos;

                            await this._itemRepository.UpdateAsync(itemEntity).ConfigureAwait(false);
                        }

                        //await _itemRepository.UpdateAsync(Item);
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Current.SaveChanges();
                    unitOfWork.Dispose();

                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoItemDto input)
        {
            try
            {
                await this._itemRepository.DeleteAsync(input.Id).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoItemDto> Obter(long id)
        {
            try
            {
                var query = await this._itemRepository
                                .GetAll()
                                .Include(i => i.Formata)
                                .Include(i => i.Material)
                                .Include(i => i.ExameInclui)
                                .Include(i => i.BrasItem)
                                .Include(i => i.Equipamento)
                                .Include(i => i.Grupo)
                                .Include(i => i.LaudoGrupo)
                                .Include(i => i.Mapa)
                                .Include(i => i.Metodo)
                                .Include(i => i.Setor)
                                .Include(i => i.SubGrupo)
                                .Include(i => i.Unidade)
                                .Where(m => m.Id == id)
                                .FirstOrDefaultAsync().ConfigureAwait(false);

                var item = FaturamentoItemDto.Mapear(query);

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
        
        public async Task<IEnumerable<FaturamentoItemDto>> ObterIds(List<long> ids)
        {
            try
            {
                var query = await this._itemRepository
                    .GetAll()
                    .Include(i => i.Formata)
                    .Include(i => i.Material)
                    .Include(i => i.ExameInclui)
                    .Include(i => i.BrasItem)
                    .Include(i => i.Equipamento)
                    .Include(i => i.Grupo)
                    .Include(i => i.LaudoGrupo)
                    .Include(i => i.Mapa)
                    .Include(i => i.Metodo)
                    .Include(i => i.Setor)
                    .Include(i => i.SubGrupo)
                    .Include(i => i.Unidade)
                    .Where(m => ids.Contains(m.Id))
                    .ToListAsync()
                    .ConfigureAwait(false);

                var item = FaturamentoItemDto.Mapear(query);

                return query.ToList().Select(x=> FaturamentoItemDto.Mapear(x)).ToList();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        

        public async Task<IEnumerable<FaturamentoItemDto>> ObterPorCodigo(ObterPorCodigoDto input)
        {
            try
            {
                if(input.Codigo.IsNullOrEmpty() && input.CodAmb.IsNullOrEmpty() && input.CodCbhpm.IsNullOrEmpty() && input.CodTuss.IsNullOrEmpty())
                {
                    return null;
                }

                var query = await this._itemRepository
                                .GetAll()
                                .Include(m => m.Grupo)
                                .WhereIf(!input.Codigo.IsNullOrEmpty(), x=> x.Codigo == input.Codigo)
                                .WhereIf(!input.CodAmb.IsNullOrEmpty(), x => x.CodAmb == input.CodAmb)
                                .WhereIf(!input.CodCbhpm.IsNullOrEmpty(), x => x.CodCbhpm == input.CodCbhpm)
                                .WhereIf(!input.CodTuss.IsNullOrEmpty(), x => x.CodTuss == input.CodTuss)
                                .ToListAsync().ConfigureAwait(false);

                return FaturamentoItemDto.Mapear(query);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<long> ObterTipoGrupoId(long? fatItemId)
        {
            if (!fatItemId.HasValue)
            {
                return 0;
            }

            try
            {
                var item = await this._itemRepository
                                .GetAll()
                                .Include(m => m.Grupo.TipoGrupo)
                                .Where(m => m.Id == fatItemId).AsNoTracking()
                                .FirstOrDefaultAsync().ConfigureAwait(false);
                if (item.Grupo == null)
                {
                    return 0;
                }
                
                return item.Grupo.TipoGrupoId.HasValue ? item.Grupo.TipoGrupo.Id : 0;
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroPesquisar"));
            }

        }

        public async Task<FaturamentoItemDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _itemRepository
                    .GetAll();

                var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                var item = FaturamentoItemDto.Mapear(result);

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoItensInput input)
        {
            try
            {
                var result = await this.Listar(input).ConfigureAwait(false);
                var itens = result.Items;
                return _listarItensExcelExporter.ExportToFile(itens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            var isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

            using (var FaturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            {
                return await this.CreateSelect2(FaturamentoItemRepository.Object)
                            .AddTextField("CONCAT(Codigo, ' - ', Descricao)")
                           .AddWhereMethod(
                               (input, dapperParameters) =>
                               {
                                   dapperParameters.Add("deleted", false);
                                   dapperParameters.Add("isLaudo", isLaudo);
                                   var whereBuilder = new StringBuilder();
                                   whereBuilder.Append("IsDeleted = @deleted");
                                   whereBuilder.Append(" AND IsLaudo = @isLaudo");

                                   whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND ((Descricao LIKE '%' + @search + '%' OR Descricao LIKE '%' + @search + '%') OR (Codigo LIKE '%' + @search + '%' OR Codigo LIKE '%' + @search + '%'))");
                                   return whereBuilder.ToString();
                               }).AddOrderByClause("Descricao").ExecuteAsync(dropdownInput)
                           .ConfigureAwait(false);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownContaMedica(DropdownInput dropdownInput)
        {
            if (dropdownInput.filtros.IsNullOrEmpty() || dropdownInput.filtros.Length != 4)
            {
                return new ResultDropdownList<long>();
            }
            var convenioId =  ParserHelper.TryParseNullable(dropdownInput.filtros[0]);
            var data = dropdownInput.filtros[1];
            var planoId = ParserHelper.TryParseNullable(dropdownInput.filtros[2]);
            var empresaId = ParserHelper.TryParseNullable(dropdownInput.filtros[3]);

            using (var FaturamentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoItem, long>>())
            {
                //var fromClause = @"FatItem 
                //    INNER JOIN 
                //    (
                //        SELECT  DISTINCT 
                //        FatConfigConvenio.FatGrupoId AS FatConfigConvenioFatGrupoId, FatConfigConvenio.FatSubGrupoId AS FatConfigConvenioFatSubGrupoId, 
                //        FatConfigConvenio.EmpresaId AS FatConfigConvenioEmpresaId, FatConfigConvenio.PlanoId AS FatConfigConvenioPlanoId
                //        FROM FatConfigConvenio
                //            LEFT JOIN SisConvenio ON FatConfigConvenio.ConvenioId = SisConvenio.Id AND SisConvenio.IsDeleted = @IsDeleted
                //            LEFT JOIN FatGrupo ON FatConfigConvenio.FatGrupoId = FatGrupo.Id AND FatGrupo.IsDeleted = @IsDeleted
                //            LEFT JOIN FatSubGrupo ON FatConfigConvenio.FatSubGrupoId = FatSubGrupo.Id AND FatSubGrupo.IsDeleted = @IsDeleted
                //            INNER JOIN FatTabela ON FatConfigConvenio.FatTabelaId = FatTabela.Id AND FatTabela.IsDeleted = @IsDeleted
                //            INNER JOIN FatItemTabela ON FatItemTabela.Id = FatTabela.Id AND FatItemTabela.IsDeleted = @IsDeleted
                //            LEFT JOIN FatItem ON FatConfigConvenio.FatItemId = FatItem.Id AND FatItem.IsDeleted = @IsDeleted
                //            WHERE 
                //            (FatConfigConvenio.ConvenioId = @convenioId OR FatConfigConvenio.ConvenioId IS NULL) 
                //            AND FatConfigConvenio.IsDeleted = @IsDeleted
                //            AND FatConfigConvenio.DataIncio <= @data AND ((FatConfigConvenio.DataFim > @data) OR (FatConfigConvenio.DataFim IS NULL))
                //    ) AS CTE_ConfigConvenio
                //    ON 
                //    (CTE_ConfigConvenio.FatConfigConvenioFatGrupoId = FatItem.GrupoId OR CTE_ConfigConvenio.FatConfigConvenioFatGrupoId = 0 OR CTE_ConfigConvenio.FatConfigConvenioFatGrupoId IS NULL)
                //    AND (CTE_ConfigConvenio.FatConfigConvenioFatSubGrupoId = FatItem.SubGrupoId OR CTE_ConfigConvenio.FatConfigConvenioFatSubGrupoId = 0 OR CTE_ConfigConvenio.FatConfigConvenioFatSubGrupoId IS NULL)
                //    AND (CTE_ConfigConvenio.FatConfigConvenioPlanoId = @planoId OR CTE_ConfigConvenio.FatConfigConvenioPlanoId = 0 OR CTE_ConfigConvenio.FatConfigConvenioPlanoId IS NULL)
                //    AND (CTE_ConfigConvenio.FatConfigConvenioEmpresaId = @empresaId OR CTE_ConfigConvenio.FatConfigConvenioPlanoId = 0 OR CTE_ConfigConvenio.FatConfigConvenioPlanoId IS NULL)";

                var fromClause = @" FatItem CROSS APPLY [FatRetornaConfigAtual](@empresaId, @convenioId, @planoId, FatItem.Id, FatItem.GrupoId, FatItem.SubGrupoId, CONVERT(datetime2, @data,103)) ";

                return await this.CreateSelect2()
                    .EnableDistinct()
                    .AddIdField("FatItem.Id")
                    .AddTextField("CONCAT(Codigo, ' - ', Descricao)")
                    .AddFromClause(fromClause)
                    .AddWhereMethod((input, dapperParameters) =>
                    {
                        dapperParameters.Add("IsDeleted", false);
                        dapperParameters.Add("convenioId", convenioId);
                        dapperParameters.Add("data", data);
                        dapperParameters.Add("planoId", planoId);
                        dapperParameters.Add("empresaId", empresaId);
                        var whereBuilder = new StringBuilder();
                        whereBuilder.Append("FatItem.IsDeleted = @IsDeleted ");
                        whereBuilder.WhereIf(!input.search.IsNullOrEmpty(), " AND ((Descricao LIKE '%' + @search + '%' OR Descricao LIKE '%' + @search + '%') OR (Codigo LIKE '%' + @search + '%' OR Codigo LIKE '%' + @search + '%'))");
                        return whereBuilder.ToString();
                    }).AddOrderByClause("FatItem.Id, CONCAT(Codigo, ' - ', Descricao)").ExecuteAsync(dropdownInput)
                .ConfigureAwait(false);
            }
        }

        public async Task<ResultDropdownListDeluxe> ListarDropdownDeluxe(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) && dropdownInput.filtro.Equals("IsLaudo");

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            ).Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItemsDeluxe
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao),
                                codigo = p.Codigo
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync().ConfigureAwait(false);

                var total = await query.CountAsync().ConfigureAwait(false);

                return new ResultDropdownListDeluxe() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownCodigo(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            ).Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo)
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdownExame(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            )
                            .Where(m =>
                                //exame laboratorial
                                m.IsLaboratorio ||
                                m.Grupo.IsLaboratorio ||
                                m.SubGrupo.IsLaboratorio ||
                                m.IsLaudo ||
                                m.Grupo.IsLaudo ||
                                m.SubGrupo.IsLaudo
                                )
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao) //string.Format("{0:D8} {1}", p.Codigo, p.Descricao)
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<ListResultDto<FaturamentoItemDto>> ListarTodos()
        {
            try
            {
                var query = _itemRepository.GetAll();

                var faturamentoItensDto = await query.ToListAsync().ConfigureAwait(false);

                return new ListResultDto<FaturamentoItemDto> { Items = FaturamentoItemDto.Mapear(faturamentoItensDto).ToList() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarFatItemDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                        )
                        .Where(f => !f.IsLaudo && !f.Grupo.IsLaudo && !f.IsLaboratorio && !f.Grupo.IsLaboratorio)
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarExameLaboratorialDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                        )
                        .Where(f => f.Grupo.IsLaboratorio || f.IsLaboratorio || f.SubGrupo.IsLaboratorio)
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarExameImagemDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            )
                            .Where(f => (f.Grupo.IsLaudo && !f.Grupo.IsLaboratorio) || (f.IsLaudo && !f.IsLaboratorio) || (f.SubGrupo.IsLaudo && !f.SubGrupo.IsLaboratorio))
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarExameDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            )
                            .Where(f => ((f.Grupo.IsLaudo && !f.Grupo.IsLaboratorio) || (f.IsLaudo && !f.IsLaboratorio) || (f.SubGrupo.IsLaudo && !f.SubGrupo.IsLaboratorio)) ||
                                        (f.Grupo.IsLaboratorio || f.IsLaboratorio || f.SubGrupo.IsLaboratorio))
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDiagnosticoImagemDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                            ).Where(f => f.IsLaudo || f.Grupo.IsLaudo || f.SubGrupo.IsLaudo)
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarFaturamentoItemPorGrupoSubGrupoDropdown(DropdownInput dropdownInput)
        {
            long grupoId;
            long subGrupoId;

            long.TryParse(dropdownInput.filtros[0], out grupoId);
            long.TryParse(dropdownInput.filtros[1], out subGrupoId);

            return await this.ListarDropdownLambda(dropdownInput
                       , this._itemRepository
                       , m => ((grupoId == 0 || m.GrupoId == grupoId)
                               && (subGrupoId == 0 || m.SubGrupoId == subGrupoId)
                           )
                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownTodos(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2(this._itemRepository).ExecuteAsync(dropdownInput).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownSemPacote(DropdownInput dropdownInput)
        {

            return await this.ListarDropdownLambda(dropdownInput
                       , this._itemRepository
                       , m => (m.Grupo.TipoGrupoId != 4
                               && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search) ||
                                   m.Codigo.Contains(dropdownInput.search))
                           )
                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarDropdownPacote(DropdownInput dropdownInput)
        {

            return await this.ListarDropdownLambda(dropdownInput
                       , this._itemRepository
                       , m => (m.Grupo.TipoGrupoId == 4
                               && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search) ||
                                   m.Codigo.Contains(dropdownInput.search))
                           )
                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }


        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarNaoLaudoNaoLaboratorioDropdown(DropdownInput dropdownInput)
        {
            var pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = 1;

            var faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.Contains(dropdownInput.search) ||
                            m.Codigo.Contains(dropdownInput.search)
                        )
                        .Where(f => !(f.Grupo.IsLaboratorio || f.IsLaboratorio || f.SubGrupo.IsLaboratorio)
                                 && !(f.Grupo.IsLaudo || f.IsLaudo || f.SubGrupo.IsLaudo)
                                 )
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
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarCirurgiaAgendamentoDropdown(DropdownInput dropdownInput)
        {

            return await this.ListarDropdownLambda(dropdownInput
                       , this._itemRepository
                       , m => (m.IsAgendaCirurgia
                               && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search) ||
                                   m.Codigo.Contains(dropdownInput.search)
                                   || m.CodTuss.Contains(dropdownInput.search)

                                  )
                           )
                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao, " - ", p.CodTuss) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarMaterialAgendamentoDropdown(DropdownInput dropdownInput)
        {

            return await this.ListarDropdownLambda(dropdownInput
                       , this._itemRepository
                       , m => (m.IsAgendaMaterial
                               && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search) ||
                                   m.Codigo.Contains(dropdownInput.search)
                                   || m.CodTuss.Contains(dropdownInput.search)

                                  )
                           )
                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao, " - ", p.CodTuss) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<IResultDropdownList<long>> ListarMateriaisOPMEDropdown(DropdownInput dropdownInput)
        {

            return await this.ListarDropdownLambda(dropdownInput
                       , this._itemRepository
                       , m => (m.IsOpme
                               && (string.IsNullOrEmpty(dropdownInput.search) || m.Descricao.Contains(dropdownInput.search) ||
                                   m.Codigo.Contains(dropdownInput.search)
                                   || m.CodTuss.Contains(dropdownInput.search)

                                  )
                           )
                       , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao, " - ", p.CodTuss) }
                       , o => o.Descricao
                   ).ConfigureAwait(false);
        }

    }

    public class ResultDropdownListDeluxe
    {
        public List<DropdownItemsDeluxe> Items { get; set; }

        public int TotalCount { get; set; }
    }

    public class DropdownItemsDeluxe
    {
        public long id { get; set; }
        public string text { get; set; }
        public string codigo { get; set; }
    }
}
