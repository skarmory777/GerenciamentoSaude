using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Collections.Extensions;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFramework.Repositories;
using Abp.EntityFramework.Uow;
using Abp.Extensions;
using Dapper;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.UltimosIds;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Enumeradores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
//using EntityFramework.BulkInsert.Extensions;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques
{
    public class InventarioAppService : SWMANAGERAppServiceBase, IInventarioAppService
    {
        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ListagemInventario>> Listar(ListarInventarioInput input)
        {

            input.StartDate = input.StartDate.HasValue ? ((DateTime)input.StartDate).Date : (DateTime?)null;
            input.EndDate = input.EndDate.HasValue ? ((DateTime)input.EndDate).Date.AddDays(1).AddSeconds(-1) : (DateTime?)null;

            input.Sorting = "inv.Id desc";

            const string DefaultField = "inv.Id";
            const string SelectClause = @"inv.Id as Id
                                        , Inv.Codigo as Numero
                                        , est.Descricao as Estoque
                                        , inv.DataInventario as DataInventario
                                        , sInv.Descricao as Status
                                        , sInv.Id as StatusId";

            const string FromClause = @"EstInventario Inv
                            join Est_Estoque est on est.Id = inv.EstoqueId
                            join EstStatusInventario sInv on sInv.Id = inv.StatusInventarioId";

            const string WhereClause = @"Inv.IsDeleted = 0
                             and ( @EstoqueId is null or inv.EstoqueId =  @EstoqueId)
                             and (@StartDate is null or inv.DataInventario >= @StartDate)
                             and (@EndDate is null or inv.DataInventario <= @EndDate)";

            try
            {
                return await this.CreateDataTable<ListagemInventario, ListarInventarioInput>()
                         .AddDefaultField(DefaultField)
                         .AddSelectClause(SelectClause)
                         .AddFromClause(FromClause)
                         .AddWhereClause(WhereClause)
                         //.AddWhereMethod(ExecutaFiltroInventario)
                         .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                         .ExecuteAsync(input).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }

            return null;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<PagedResultDto<ListagemInventarioEstoqueContagem>> ListarProdutoEstoque(ListarInventarioEstoqueInput input)
        {
            input.Sorting = "pro.DescricaoResumida";

            const string DefaultField = "itm.Id";
            const string SelectClause = @"pro.DescricaoResumida as ProdutoDescricao,
	                                       lv.Lote as Lote,
	                                       lv.Validade,
										   itm.LoteValidadeId as LoteValidadeId,
										   itm.QuantidadeEstoque as  QuantidadeEstoque";

            const string FromClause = @"Est_Produto pro
				join EstInventarioItem itm on itm.ProdutoId = pro.Id
				join EstInventario ivm on ivm.Id = itm.InventarioId
				 join LoteValidade lv on lv.Id = itm.LoteValidadeId";

            const string WhereClause = @"ivm.Id = @InventarioId";

            try
            {

                var listagemInventarioEstoqueIndex = await this.CreateDataTable<ListagemInventarioEstoqueContagem, ListarInventarioEstoqueInput>()
                            .AddDefaultField(DefaultField)
                            .AddSelectClause(SelectClause)
                            .AddFromClause(FromClause)
                            .AddWhereClause(WhereClause)
                            //.AddWhereMethod(ExecutaFiltroInventario)
                            .AddDefaultErrorMessage(this.L("ErroPesquisar"))
                            .ExecuteAsync(input).ConfigureAwait(false);

                return listagemInventarioEstoqueIndex;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private async Task<IEnumerable<ListagemInventarioEstoque>> RetornaProdutosEstoque(long estoqueId, long? grupoId, long? classeId, long? subClasseId)
        {
            const string SelectClause = @"pro.Id as ProdutoId,
                                                pro.DescricaoResumida AS ProdutoDescricao,
	                                            lv.Lote AS Lote,
	                                            lv.Validade,
                                                ps.LoteValidadeId AS LoteValidadeId,
                                                ps.QuantidadeAtual AS QuantidadeEstoque";

            const string FromClause = @"Est_Produto pro
                                        join ProdutoSaldo ps ON ps.ProdutoId = pro.Id
                                        left join LoteValidade lv ON lv.Id = ps.LoteValidadeId";

            const string WhereClause = @"ps.EstoqueId = @EstoqueId
                                    and (@GrupoId is null OR @GrupoId = pro.GrupoId)
                                    and (@ClasseId is null OR @ClasseId = pro.GrupoClasseId)
                                    and (@SubClasseId is null OR @SubClasseId = pro.GrupoSubClasseId)
                                    and ps.QuantidadeAtual > 0 AND pro.IsDeleted = 0
                                order by ProdutoDescricao";
            var query = string.Format("select {0} from {1} where {2}", SelectClause, FromClause, WhereClause);

            using (var connection = new SqlConnection(this.GetConnection()))
            {
                return (await connection.QueryAsync<ListagemInventarioEstoque>(query, new
                {
                    EstoqueId = estoqueId,
                    GrupoId = grupoId,
                    ClasseId = classeId,
                    SubClasseId = subClasseId
                }));
            }
        }
        public async Task<DefaultReturn<InventarioDto>> GerarInventario(long estoqueId, long? grupoId, long? classeId, long? subClasseId, long? id)
        {
            DefaultReturn<InventarioDto> defaultReturn = new DefaultReturn<InventarioDto>
            {
                Errors = new List<ErroDto>()
            };


            using (var ultimoIdAppService = IocManager.Instance.ResolveAsDisposable<IUltimoIdAppService>())
            using (var inventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Inventario, long>>())
            using (var statusInventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<StatusInventario, long>>())
            using (var statusInventarioItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<StatusInventarioItem, long>>())
            using (var _unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
            using (var unitOfWork = _unitOfWorkManager.Object.Begin())
            {
                try
                {
                    var produtosEstoque = await this.RetornaProdutosEstoque(estoqueId, grupoId, classeId, subClasseId).ConfigureAwait(false);
                    var statusInventarios = await statusInventarioRepository.Object.GetAll().AsNoTracking().ToListAsync();
                    var statusInventarioItems = await statusInventarioItemRepository.Object.GetAll().AsNoTracking().ToListAsync();

                    if (id == null || id == 0)
                    {
                        var inventario = new Inventario
                        {
                            Codigo = await ultimoIdAppService.Object.ObterProximoCodigo("Inventario"),
                            DataInventario = DateTime.Now,
                            EstoqueId = estoqueId,
                            StatusInventarioId = StatusInventario.Inicial,
                            TipoInventarioId = (long)EnumTipoInventario.Conferencia
                        };

                        var itens = new List<InventarioItem>();

                        foreach (var item in produtosEstoque)
                        {
                            var inventarioItem = new InventarioItem
                            {
                                LoteValidadeId = item.LoteValidadeId,
                                ProdutoId = item.ProdutoId,
                                QuantidadeEstoque = item.QuantidadeEstoque,
                                StatusInventarioItemId = StatusInventarioItem.Inicial
                            };

                            itens.Add(inventarioItem);
                        }

                        inventario.Itens = itens;

                        var inventarioId = inventarioRepository.Object.InsertAndGetId(inventario);

                        long gridId = 1;

                        // var listaItens = itens.Select(s => new ListagemInventarioEstoqueContagem { GridId = gridId++, Id = s.Id, ProdutoDescricao = produtosEstoque.Where(w => w.ProdutoId == s.ProdutoId).Select(s2 => s2.ProdutoDescricao).FirstOrDefault(), Lote = produtosEstoque.Where(w => w.LoteValidadeId == s.LoteValidadeId).Select(s2 => s2.Lote).FirstOrDefault(), Validade = produtosEstoque.Where(w => w.LoteValidadeId == s.LoteValidadeId).Select(s2 => s2.Validade).FirstOrDefault() });


                        var listaItens = new List<ListagemInventarioEstoqueContagem>();

                        foreach (var item in itens)
                        {
                            var itemInventario = new ListagemInventarioEstoqueContagem
                            {
                                GridId = gridId++,
                                ItemId = item.Id,
                                StatusInventarioItemId = item.StatusInventarioItemId,
                                StatusInventarioItemDescricao = statusInventarioItems.FirstOrDefault(x => x.Id == item.StatusInventarioItemId)?.Descricao,
                                ProdutoDescricao = produtosEstoque.Where(w => w.ProdutoId == item.ProdutoId).Select(s2 => s2.ProdutoDescricao).FirstOrDefault(),
                                Lote = produtosEstoque.Where(w => w.LoteValidadeId == item.LoteValidadeId).Select(s2 => s2.Lote).FirstOrDefault(),
                                Validade = produtosEstoque.Where(w => w.LoteValidadeId == item.LoteValidadeId).Select(s2 => s2.Validade).FirstOrDefault()
                            };

                            listaItens.Add(itemInventario);
                        }

                        var inventarioDto = InventarioDto.Mapear(inventario);

                        inventarioDto.Status = statusInventarios.FirstOrDefault(x => x.Id == inventarioDto.StatusInventarioId)?.Descricao;

                        inventarioDto.ItensJson = JsonConvert.SerializeObject(listaItens);

                        inventarioDto.Id = inventarioId;

                        defaultReturn.ReturnObject = inventarioDto;
                    }
                    else
                    {
                        var inventario = inventarioRepository.Object.GetAll().Include(i => i.Itens).Where(w => w.Id == id).FirstOrDefault();

                        if (inventario != null)
                        {
                            var novosItens = produtosEstoque.Where(w => !inventario.Itens.Any(a => a.ProdutoId == w.ProdutoId && a.LoteValidadeId == w.LoteValidadeId));

                            foreach (var item in novosItens)
                            {
                                var inventarioItem = new InventarioItem
                                {
                                    LoteValidadeId = item.LoteValidadeId,
                                    ProdutoId = item.ProdutoId,
                                    StatusInventarioItemId = StatusInventarioItem.Inicial
                                };

                                inventario.Itens.Add(inventarioItem);
                            }

                            inventarioRepository.Object.Update(inventario);

                            var inventarioDto = InventarioDto.Mapear(inventario);
                            long gridId = 1;
                            var listaItens = inventario.Itens.Select(s => new ListagemInventarioEstoqueContagem
                            {
                                ItemId = s.Id,
                                GridId = gridId++,
                                StatusInventarioItemId = s.StatusInventarioItemId,
                                StatusInventarioItemDescricao = statusInventarioItems.FirstOrDefault(x => x.Id == s.StatusInventarioItemId)?.Descricao,
                                ProdutoDescricao = produtosEstoque.Where(w => w.ProdutoId == s.ProdutoId).Select(s2 => s2.ProdutoDescricao).FirstOrDefault(),
                                Lote = produtosEstoque.Where(w => w.LoteValidadeId == s.LoteValidadeId).Select(s2 => s2.Lote).FirstOrDefault(),
                                Validade = produtosEstoque.Where(w => w.LoteValidadeId == s.LoteValidadeId).Select(s2 => s2.Validade).FirstOrDefault()
                            }).OrderBy(o => o.ProdutoDescricao).ToList();

                            //foreach (var item in listaItens)
                            //{
                            //    item.GridId = gridId++;
                            //}

                            inventarioDto.ItensJson = JsonConvert.SerializeObject(listaItens);

                            inventarioDto.Status = statusInventarios.FirstOrDefault(x => x.Id == inventarioDto.StatusInventarioId)?.Descricao;
                            defaultReturn.ReturnObject = inventarioDto;
                        }
                    }

                    unitOfWork.Complete();
                    _unitOfWorkManager.Object.Current.SaveChanges();
                    unitOfWork.Dispose();

                }
                catch (Exception ex)
                {

                }
            }
            return defaultReturn;
        }

        [DisableAuditing]
        [UnitOfWork(false)]
        public async Task<InventarioDto> Obter(long id)
        {
            using (var connection = new SqlConnection(this.GetConnection()))
            using (var inventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Inventario, long>>())
            {

                const string SelectClause = @"inv.Id,
                                              inv.DataInventario,
                                              inv.Codigo,
                                              est.Descricao as EstoqueDescricao,
                                              sInv.Descricao as Status,
                                              inv.StatusInventarioId as StatusInventarioId
                                              ";

                const string FromClause = @"EstInventario inv
                                          join EstStatusInventario sInv on sInv.Id = inv.StatusInventarioId
                                          join Est_Estoque est on est.Id = inv.EstoqueId";

                const string WhereClause = "inv.Id = @Id";

                var query = string.Format("select {0} from {1} where {2}", SelectClause, FromClause, WhereClause);

                try
                {
                    return await connection.QueryFirstAsync<InventarioDto>(query, new { Id = id, isDeleted = false });
                }
                catch (Exception ex)
                {

                }

                return null;
            }
        }

        public async Task<PagedResultDto<InventarioEstoque>> ListarItensTodosPorInventario(InventarioEstoqueListarInput input)
        {
            return await ListarItemsPorInventario(input).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<InventarioEstoque>> ListarItensContadosPorInventario(InventarioEstoqueListarInput input)
        {
            return await ListarItemsPorInventario(input, new List<long> { StatusInventarioItem.PrimeiraContagem, StatusInventarioItem.SegundaContagem, StatusInventarioItem.Fechado }).ConfigureAwait(false);
        }

        public async Task<PagedResultDto<InventarioEstoque>> ListarItensPendentesPorInventario(InventarioEstoqueListarInput input)
        {
            return await ListarItemsPorInventario(input, new List<long> { StatusInventarioItem.Inicial }).ConfigureAwait(false);
        }

        private async Task<PagedResultDto<InventarioEstoque>> ListarItemsPorInventario(InventarioEstoqueListarInput input, List<long> statusInventarioItemIds = null)
        {
            if (input == null || input.Id.IsNullOrEmpty() || input.Id == "0")
            {
                return new PagedResultDto<InventarioEstoque>();
            }

            using (var connection = new SqlConnection(this.GetConnection()))
            using (var inventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Inventario, long>>())
            {
                try
                {
                    const string SelectClause = @"
                                              ivItem.Id AS InvItemId,
                                              inv.Id AS Id,
                                              inv.DataInventario,
                                              inv.Codigo,
                                              est.Descricao as EstoqueDescricao,
                                              sInv.Descricao as Status,
                                              sInvItem.Id as StatusInventarioItemId,
                                              sInvItem.Descricao as StatusInventarioItemDescricao,
                                              ivItem.TemDivergencia,
                                              ivItem.ProdutoId,
                                              ivItem.LoteValidadeId,
                                              pro.DescricaoResumida as ProdutoDescricao,
                                              lv.Lote,
                                              lv.Validade,
                                              ivItem.Id as ItemId,
                                              ivItem.QuantidadeContagem";

                    const string FromClause = @"EstInventario inv
                                          JOIN EstInventarioItem ivItem on ivItem.InventarioId = inv.Id
                                          JOIN EstStatusInventario sInv on sInv.Id = inv.StatusInventarioId
                                          JOIN EstStatusInventarioItem sInvItem on SInvItem.id = ivItem.statusInventarioItemId
                                          join Est_Produto pro on pro.Id = ivItem.ProdutoId
                                          join Est_Estoque est on est.Id = inv.EstoqueId
                                          left join LoteValidade lv on lv.Id = ivItem.LoteValidadeId";

                    string WhereClause = "AND inv.Id = @Id AND ivItem.isDeleted = @isDeleted ";

                    return await this.CreateDataTable<InventarioEstoque, InventarioEstoqueListarInput>()
                         .AddDefaultField("ivItem.Id")
                         .EnablePagination(false)
                         .AddSelectClause(SelectClause)
                         .AddFromClause(FromClause)
                         .AddWhereMethod((dto, dapperParameters) =>
                         {
                             dapperParameters.Add("isDeleted", false);
                             if (!statusInventarioItemIds.IsNullOrEmpty())
                             {
                                 dapperParameters.Add("statusInventarioItemIds", statusInventarioItemIds);
                                 WhereClause += "  AND ivItem.StatusInventarioItemId IN @statusInventarioItemIds ";
                             }
                             return WhereClause;
                         }).ExecuteAsync(input);
                }
                catch (Exception ex)
                {

                }

                return null;
            }
        }


        public async Task<DashboardInventarioEstoque> DashboardInventarioEstoque(long id)
        {
            var sql = @"SELECT COUNT(StatusInventarioItemId) AS TotalItems  FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id;
            SELECT COUNT(StatusInventarioItemId) AS TotalContado  FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id AND StatusInventarioItemId IN(@PrimeiraContagem,@SegundaContagem,@Fechado);
            SELECT COUNT(StatusInventarioItemId) AS TotalPendente FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id AND StatusInventarioItemId = @Inicial;
            SELECT COUNT(StatusInventarioItemId) AS TotalPrimeiraContagem  FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id AND StatusInventarioItemId = @PrimeiraContagem;
            SELECT COUNT(StatusInventarioItemId) AS TotalSegundaContagem  FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id AND StatusInventarioItemId = @SegundaContagem;
            SELECT COUNT(StatusInventarioItemId) AS TotalDivergente  FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id AND TemDivergencia = @True AND DivergenciaResolvida = @false;
            SELECT COUNT(StatusInventarioItemId) AS TotalFechado  FROM EstInventarioItem WHERE IsDeleted = @IsDeleted AND InventarioId = @id AND StatusInventarioItemId = @Fechado;";

            using (var connection = new SqlConnection(this.GetConnection()))
            {
                var parameters = new
                {
                    IsDeleted = 0,
                    id,
                    StatusInventarioItem.Inicial,
                    StatusInventarioItem.PrimeiraContagem,
                    StatusInventarioItem.SegundaContagem,
                    StatusInventarioItem.Fechado,
                    True = 1,
                    False = 0,
                };
                using (var dataResult = await connection.QueryMultipleAsync(sql, parameters).ConfigureAwait(false))
                {
                    return new DashboardInventarioEstoque()
                    {
                        TotalItems = dataResult.ReadFirst<long>(),
                        TotalContado = dataResult.ReadFirst<long>(),
                        TotalPendente = dataResult.ReadFirst<long>(),
                        TotalPrimeiraContagem = dataResult.ReadFirst<long>(),
                        TotalSegundaContagem = dataResult.ReadFirst<long>(),
                        TotalDivergente = dataResult.ReadFirst<long>(),
                        TotalFechado = dataResult.ReadFirst<long>(),
                    };
                }
            }
        }

        public class InventarioEstoqueListarInput : ListarInput
        {
            public override void Normalize()
            {
                if (Sorting.IsNullOrWhiteSpace())
                {
                    Sorting = "pro.DescricaoResumida ASC";
                }
            }
        }

        public async Task<DefaultReturn<InventarioDto>> Atualizar(long id, List<ListagemInventarioEstoqueContagem> itens)
        {
            var defaultReturn = new DefaultReturn<InventarioDto>
            {
                Warnings = new List<ErroDto>()
            };

            bool existeDivergencia = false;

            using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            using (var inventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Inventario, long>>())
            {
                var inventario = inventarioRepository.Object.GetAll().Include(i => i.Itens).Where(w => w.Id == id).FirstOrDefault();
                if (inventario != null && inventario.Itens != null)
                {
                    var enableStatusInventarioItens = new[]
                    {
                        StatusInventarioItem.Inicial,
                        StatusInventarioItem.PrimeiraContagem,
                        StatusInventarioItem.SegundaContagem,
                    };

                    switch (inventario.StatusInventarioId)
                    {
                        case StatusInventario.Inicial:
                            foreach (var item in itens.Where(x => enableStatusInventarioItens.Contains(x.StatusInventarioItemId)))
                            {
                                var inventarioItem = inventario.Itens.Where(w => w.Id == item.ItemId).FirstOrDefault();
                                if (inventarioItem != null)
                                {
                                    inventarioItem.QuantidadeContagem = item.QuantidadeContagem;
                                    // TODO: Comentado para quando houver divergencia.
                                    //CheckDivergencia(inventarioItem, item.QuantidadeContagem);

                                    inventarioItem.StatusInventarioItemId = StatusInventarioItem.PrimeiraContagem;
                                }

                                if (item.ItemId == 0 && item.ProdutoId != null)
                                {
                                    var novoInventarioItem = new InventarioItem
                                    {
                                        ProdutoId = (long)item.ProdutoId,
                                        QuantidadeContagem = item.QuantidadeContagem,
                                        StatusInventarioItemId = StatusInventarioItem.Inicial
                                    };

                                    if (item.Validade != null)
                                    {
                                        var loteValidade = estoqueLoteValidadeAppService.Object.Obter((long)item.ProdutoId, item.Lote, (DateTime)item.Validade, null);
                                        if (loteValidade != null)
                                        {
                                            novoInventarioItem.LoteValidadeId = loteValidade.Id;
                                        }
                                        else
                                        {
                                            var novoLoteValidade = new LoteValidade
                                            {
                                                Lote = item.Lote,
                                                Validade = (DateTime)item.Validade,
                                                ProdutoId = (long)item.ProdutoId
                                            };
                                            novoInventarioItem.LoteValidadeId = await loteValidadeRepository.Object.InsertAndGetIdAsync(novoLoteValidade).ConfigureAwait(false);

                                        }
                                    }
                                    inventario.Itens.Add(novoInventarioItem);
                                }
                            }
                            break;
                    }

                    await inventarioRepository.Object.UpdateAsync(inventario);
                }
            }

            if (existeDivergencia)
            {
                defaultReturn.Warnings.Add(new ErroDto { CodigoErro = "INV0001" });
            }

            return defaultReturn;
        }

        public async Task<DefaultReturn<InventarioEstoque>> AtualizarItem(long id, ListagemInventarioEstoqueContagem item)
        {
            var defaultReturn = new DefaultReturn<InventarioEstoque>
            {
                Warnings = new List<ErroDto>()
            };
            try
            {

                using (var loteValidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<LoteValidade, long>>())
                using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
                using (var inventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Inventario, long>>())
                using (var inventarioItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<InventarioItem, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var inventario = inventarioRepository.Object.GetAll().AsNoTracking().Where(w => w.Id == id).FirstOrDefault();
                    InventarioItem inventarioItem = null;
                    switch (inventario.StatusInventarioId)
                    {
                        case StatusInventario.Inicial:
                            {
                                if (item.ItemId == 0 && item.ProdutoId != null)
                                {
                                    inventarioItem = await NovoItem(item, estoqueLoteValidadeAppService.Object, loteValidadeRepository.Object).ConfigureAwait(false);
                                    inventarioItem.InventarioId = id;
                                }
                                else
                                {
                                    inventarioItem = await inventarioItemRepository.Object.GetAll().FirstOrDefaultAsync(w => w.Id == item.ItemId);
                                    if (inventarioItem != null)
                                    {
                                        inventarioItem.QuantidadeContagem = item.QuantidadeContagem;
                                        // TODO: Comentado para quando houver divergencia.
                                        //CheckDivergencia(inventarioItem);

                                        inventarioItem.StatusInventarioItemId = StatusInventarioItem.PrimeiraContagem;


                                    }

                                }
                                break;
                            }
                    }

                    if (inventarioItem != null)
                    {
                        inventarioItem.Id = await inventarioItemRepository.Object.InsertOrUpdateAndGetIdAsync(inventarioItem).ConfigureAwait(false);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();

                        using (var connection = new SqlConnection(this.GetConnection()))
                        {
                            const string query = @"
                                                SELECT ivItem.Id AS InvItemId,
                                                  inv.Id AS Id, inv.DataInventario,
                                                  inv.Codigo, est.Descricao as EstoqueDescricao,
                                                  sInv.Descricao as Status, sInvItem.Id as StatusInventarioItemId,
                                                  sInvItem.Descricao as StatusInventarioItemDescricao, ivItem.TemDivergencia,
                                                  ivItem.ProdutoId, ivItem.LoteValidadeId,
                                                  pro.DescricaoResumida as ProdutoDescricao, lv.Lote, lv.Validade, ivItem.Id as ItemId, ivItem.QuantidadeContagem
                                                FROM EstInventario inv
                                                  JOIN EstInventarioItem ivItem on ivItem.InventarioId = inv.Id
                                                  JOIN EstStatusInventario sInv on sInv.Id = inv.StatusInventarioId
                                                  JOIN EstStatusInventarioItem sInvItem on SInvItem.id = ivItem.statusInventarioItemId
                                                  join Est_Produto pro on pro.Id = ivItem.ProdutoId
                                                  join Est_Estoque est on est.Id = inv.EstoqueId
                                                  left join LoteValidade lv on lv.Id = ivItem.LoteValidadeId
                                                WHERE inv.Id = @Id AND ivItem.isDeleted = @isDeleted and ivItem.Id = @sInvId";

                            defaultReturn.ReturnObject = await connection.QueryFirstAsync<InventarioEstoque>(query, new { id, isDeleted = false, sInvId = inventarioItem.Id }).ConfigureAwait(false);
                        }
                    }
                }
            }
            catch (Exception e)
            {

            }
            return defaultReturn;
        }

        public static void CheckDivergencia(InventarioItem inventarioItem)
        {
            if (inventarioItem.QuantidadeContagem != inventarioItem.QuantidadeEstoque)
            {
                inventarioItem.TemDivergencia = true;
            }
            else
            {
                inventarioItem.TemDivergencia = false;
            }
        }

        public async Task ExcluirItem(long id, ListagemInventarioEstoqueContagem item)
        {
            var defaultReturn = new DefaultReturn<InventarioDto>
            {
                Warnings = new List<ErroDto>()
            };
            try
            {
                if (item.ItemId != 0)
                {
                    using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                    using (var unitOfWork = unitOfWorkManager.Object.Begin())
                    using (var inventarioItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<InventarioItem, long>>())
                    {
                        await inventarioItemRepository.Object.DeleteAsync(item.ItemId);
                        unitOfWork.Complete();
                        unitOfWorkManager.Object.Current.SaveChanges();
                        unitOfWork.Dispose();
                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        private static async Task<InventarioItem> NovoItem(ListagemInventarioEstoqueContagem item,
            IEstoqueLoteValidadeAppService estoqueLoteValidadeAppService,
            IRepository<LoteValidade, long> loteValidadeRepository)
        {
            var novoInventarioItem = new InventarioItem
            {
                ProdutoId = (long)item.ProdutoId,
                QuantidadeContagem = item.QuantidadeContagem,
                StatusInventarioItemId = StatusInventarioItem.PrimeiraContagem
            };

            if (item.Validade != null)
            {
                var loteValidade = estoqueLoteValidadeAppService.Obter((long)item.ProdutoId, item.Lote, (DateTime)item.Validade, null);
                if (loteValidade != null)
                {
                    novoInventarioItem.LoteValidadeId = loteValidade.Id;
                }
                else
                {
                    var novoLoteValidade = new LoteValidade
                    {
                        Lote = item.Lote,
                        Validade = (DateTime)item.Validade,
                        ProdutoId = (long)item.ProdutoId
                    };
                    novoInventarioItem.LoteValidadeId = await loteValidadeRepository.InsertAndGetIdAsync(novoLoteValidade);

                }

            }
            return novoInventarioItem;
        }

        [UnitOfWork(false)]
        public async Task<DefaultReturn<InventarioDto>> FecharInventario(long id)
        {
            var defaultReturn = new DefaultReturn<InventarioDto>
            {
                Errors = new List<ErroDto>()
            };

            try
            {
                using (var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>())
                using (var estoquePreMovimentoItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimentoItem, long>>())
                using (var estoqueMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueMovimento, long>>())
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                using (var unitOfWorkManager = IocManager.Instance.ResolveAsDisposable<IUnitOfWorkManager>())
                using (var unitOfWork = unitOfWorkManager.Object.Begin())
                {
                    var connection = produtoSaldoRepository.Object.GetDbContext().Database.Connection;
                    produtoSaldoRepository.Object.GetDbContext().Configuration.AutoDetectChangesEnabled = false;
                    produtoSaldoRepository.Object.GetDbContext().Configuration.ValidateOnSaveEnabled = false;

                    var queryInventario = $@" 
                                    SELECT {QueryHelper.CreateQueryFields<Inventario>("Inventario").GetFields()}
                                    FROM EstInventario AS Inventario
                                    WHERE Inventario.Id = @id;
                                    SELECT
                                        {QueryHelper.CreateQueryFields<InventarioItem>("InventarioItem")
                                        .IgnoreField(x => x.UsuarioContegem)
                                        .IgnoreField(x => x.UsuarioContegem2)
                                        .IgnoreField(x => x.UsuarioContegem3)
                                        .IgnoreField(x => x.UsuarioDivergencia)
                                        .GetFields()}
                                    FROM EstInventarioItem AS InventarioItem
                                    WHERE InventarioItem.InventarioId = @id AND InventarioItem.isDeleted = @False 
                    ";

                    Inventario inventario = null;
                    using (var multiple = await connection.QueryMultipleAsync(queryInventario, new { id, False = false, True = true }))
                    {
                        inventario = await multiple.ReadFirstOrDefaultAsync<Inventario>();

                        if (inventario != null)
                        {
                            inventario.Itens = (await multiple.ReadAsync<InventarioItem>()).ToList();
                        }
                    }

                    var empresaPrincipalEstoque = await this.ObterEmpresaPrincipalEstoque(inventario.EstoqueId);
                    var listaEntrada = new List<InventarioItem>();
                    var listaSaida = new List<InventarioItem>();
                    var listaItems = new List<InventarioItem>();
                    if (inventario != null && inventario.Itens != null)
                    {
                        listaSaida.AddRange(inventario.Itens.Where(item => item.QuantidadeContagem != null && (item.QuantidadeEstoque ?? 0) > item.QuantidadeContagem));

                        listaEntrada.AddRange(inventario.Itens.Where(item => item.QuantidadeContagem != null && (item.QuantidadeEstoque ?? 0) < item.QuantidadeContagem));
                        listaItems.AddRange(listaSaida);
                        listaItems.AddRange(listaEntrada);
                        listaItems = listaItems.Distinct().ToList();

                        var produtoIds = listaEntrada.Select(s => s.ProdutoId).Concat(listaSaida.Select(s => s.ProdutoId)).Distinct().ToList();

                        var loteValidadeIds = listaEntrada.Select(s => s.LoteValidadeId).Concat(listaSaida.Select(s => s.LoteValidadeId)).Where(x => x.HasValue).Distinct().ToList();

                        var queryUnidades = $@" 
                                    SELECT {QueryHelper.CreateQueryFields<ProdutoUnidade>("ProdutoUnidade").GetFields()}
                                    FROM Est_ProdutoUnidade  AS ProdutoUnidade
                                    WHERE ProdutoUnidade.ProdutoId IN @produtoIds AND ProdutoUnidade.UnidadeTipoId = @unidadeTipoId";

                        var unidades = await connection.QueryAsync<ProdutoUnidade>(queryUnidades, new { produtoIds, unidadeTipoId = 1 });

                        var query = $@" SELECT {QueryHelper.CreateQueryFields<ProdutoSaldo>().GetFields()} FROM ProdutoSaldo 
                                    WHERE [ProdutoSaldo].[ProdutoId] IN @produtoIds AND [ProdutoSaldo].[EstoqueId] = @estoqueId";

                        if (loteValidadeIds.Any())
                        {
                            query += " AND [ProdutoSaldo].[LoteValidadeId] IN @loteValidadeIds";
                        }

                        var produtosSaldos = await connection.QueryAsync<ProdutoSaldo>(
                            query,
                            new
                            {
                                produtoIds,
                                estoqueId = inventario.EstoqueId,
                                loteValidadeIds
                            });
                        var produtosAtualizarInserir = new List<ProdutoSaldo>();
                        var estoquePreMovimentoItemAtualizarInserir = new List<EstoquePreMovimentoItem>();
                        var estoqueMovimentoItemAtualizarInserir = new List<EstoqueMovimentoItem>();

                        if (listaEntrada.Count > 0)
                        {
                            var estoquePreMovimento = new EstoquePreMovimento
                            {
                                Documento = inventario.Codigo,
                                Movimento = DateTime.Now,
                                Emissao = DateTime.Now,
                                IsEntrada = true,
                                EstoqueId = inventario.EstoqueId,
                                EstTipoMovimentoId = (long)EnumTipoMovimento.Inventario_Entrada,
                                EmpresaId = empresaPrincipalEstoque,
                                EstTipoOperacaoId = (long)EnumTipoOperacao.Inventario,
                                InventarioId = id,
                                PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Conferencia,
                                GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos,

                                Itens = new List<EstoquePreMovimentoItem>()
                            };

                            var (estoquePreMovimentoResultado, estoquePreMovimentoItemResultado, produtoSaldoResultado) = FechaInventarioPorMovimentacao(estoquePreMovimento, inventario, listaEntrada, unidades, produtosSaldos);
                            produtosAtualizarInserir.AddRange(produtoSaldoResultado);

                            estoquePreMovimentoResultado.Id = await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(estoquePreMovimentoResultado);

                            var estoqueMovimento = GerarMovimento(estoquePreMovimentoResultado, estoqueMovimentoRepository.Object);

                            foreach (var estoquePreMovimentoItem in estoquePreMovimentoItemResultado)
                            {
                                estoquePreMovimentoItem.PreMovimentoId = estoquePreMovimentoResultado.Id;
                            }

                            estoquePreMovimentoItemAtualizarInserir.AddRange(estoquePreMovimentoItemResultado);

                            var estoqueMovimentoItemResultado = GerarMovimentoItens(estoqueMovimento, estoquePreMovimentoItemResultado);
                            estoqueMovimentoItemAtualizarInserir.AddRange(estoqueMovimentoItemResultado);
                        }

                        if (defaultReturn.Errors.Count == 0 && listaSaida.Count > 0)
                        {
                            var estoquePreMovimento = new EstoquePreMovimento
                            {
                                Documento = inventario.Codigo,
                                Movimento = DateTime.Now,
                                Emissao = DateTime.Now,
                                IsEntrada = false,
                                EstoqueId = inventario.EstoqueId,
                                EstTipoMovimentoId = (long)EnumTipoMovimento.Inventario_Saida,
                                EmpresaId = empresaPrincipalEstoque,
                                EstTipoOperacaoId = (long)EnumTipoOperacao.Inventario,
                                InventarioId = id,
                                PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Conferencia,
                                GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos,

                                Itens = new List<EstoquePreMovimentoItem>()
                            };

                            var (estoquePreMovimentoResultado, estoquePreMovimentoItemResultado, produtoSaldoResultado) = FechaInventarioPorMovimentacao(estoquePreMovimento, inventario, listaSaida, unidades, produtosSaldos);
                            produtosAtualizarInserir.AddRange(produtoSaldoResultado);

                            estoquePreMovimentoResultado.Id = await estoquePreMovimentoRepository.Object.InsertAndGetIdAsync(estoquePreMovimentoResultado);

                            var estoqueMovimento = GerarMovimento(estoquePreMovimentoResultado, estoqueMovimentoRepository.Object);

                            foreach (var estoquePreMovimentoItem in estoquePreMovimentoItemResultado)
                            {
                                estoquePreMovimentoItem.PreMovimentoId = estoquePreMovimentoResultado.Id;
                            }

                            estoquePreMovimentoItemAtualizarInserir.AddRange(estoquePreMovimentoItemResultado);

                            var estoqueMovimentoItemResultado = GerarMovimentoItens(estoqueMovimento, estoquePreMovimentoItemResultado);
                            estoqueMovimentoItemAtualizarInserir.AddRange(estoqueMovimentoItemResultado);
                        }

                        if (connection.State != System.Data.ConnectionState.Open)
                        {
                            connection.Open();
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                QueryHelper.BulkData<EstoquePreMovimentoItem>().UserId(AbpSession.UserId).AutoCloseTransaction(false).Transaction(transaction)
                                    .BulkMerge(connection, estoquePreMovimentoItemAtualizarInserir);

                                var estoquePreMovimentoItems = connection.Query<EstoquePreMovimentoItem>(
                                    $@" SELECT {QueryHelper.CreateQueryFields<EstoquePreMovimentoItem>("EstoquePreMovimentoItem").GetFields()} 
                                        FROM EstoquePreMovimentoItem 
                                        WHERE EstoquePreMovimentoItem.PreMovimentoId IN @PreMovimentoIds",
                                    new { PreMovimentoIds = estoquePreMovimentoItemAtualizarInserir.Select(x => x.PreMovimentoId).Distinct().ToList() },
                                    transaction: transaction);
                                var estoquePreMovimentosLoteValidades = new List<EstoquePreMovimentoLoteValidade>();
                                foreach (var estoquePreMovimento in estoquePreMovimentoItems)
                                {
                                    var item = estoquePreMovimentoItemAtualizarInserir.FirstOrDefault(
                                        x => x.PreMovimentoId == estoquePreMovimento.PreMovimentoId
                                        && x.InventarioItemId == estoquePreMovimento.InventarioItemId
                                        && x.Quantidade == estoquePreMovimento.Quantidade
                                        && x.ProdutoId == estoquePreMovimento.ProdutoId);

                                    if (item != null && !item.EstoquePreMovimentosLoteValidades.IsNullOrEmpty())
                                    {
                                        item.Id = estoquePreMovimento.Id;
                                        foreach (var estoquePreMovimentosLoteValidade in item.EstoquePreMovimentosLoteValidades)
                                        {
                                            estoquePreMovimentosLoteValidade.EstoquePreMovimentoItemId = item.Id;
                                        }

                                        estoquePreMovimentosLoteValidades.AddRange(item.EstoquePreMovimentosLoteValidades);
                                    }
                                }

                                QueryHelper.BulkData<EstoquePreMovimentoLoteValidade>()
                                    .UserId(AbpSession.UserId)
                                    .AutoCloseTransaction(false)
                                    .Transaction(transaction)
                                    .BulkMerge(connection, estoquePreMovimentosLoteValidades);

                                foreach (var estoqueMovimento in estoqueMovimentoItemAtualizarInserir)
                                {
                                    var item = estoquePreMovimentoItems.FirstOrDefault(x =>
                                        x.NumeroSerie == estoqueMovimento.NumeroSerie
                                        && x.InventarioItemId == estoqueMovimento.InventarioItemId
                                        && x.ProdutoId == estoqueMovimento.ProdutoId
                                        && x.ProdutoUnidadeId == estoqueMovimento.ProdutoUnidadeId
                                        && x.Quantidade == estoqueMovimento.Quantidade);

                                    if (item != null)
                                    {
                                        estoqueMovimento.EstoquePreMovimentoItemId = item.Id;
                                    }
                                }

                                QueryHelper.BulkData<EstoqueMovimentoItem>()
                                .UserId(AbpSession.UserId)
                                .AutoCloseTransaction(false)
                                .Transaction(transaction)
                                .BulkMerge(connection, estoqueMovimentoItemAtualizarInserir);

                                var estoqueMovimentos = connection.Query<EstoqueMovimentoItem>(
                                    $@" SELECT {QueryHelper.CreateQueryFields<EstoqueMovimentoItem>("EstoqueMovimentoItem").GetFields()} 
                                        FROM EstoqueMovimentoItem 
                                        WHERE EstoqueMovimentoItem.MovimentoId IN @MovimentoIds",
                                    new { MovimentoIds = estoqueMovimentoItemAtualizarInserir.Select(x => x.MovimentoId).Distinct().ToList() },
                                    transaction: transaction);
                                var estoqueMovimentosLoteValidades = new List<EstoqueMovimentoLoteValidade>();

                                foreach (var estoqueMovimento in estoqueMovimentos)
                                {
                                    var item = estoqueMovimentoItemAtualizarInserir.FirstOrDefault(
                                        x => x.MovimentoId == estoqueMovimento.MovimentoId
                                        && x.InventarioItemId == estoqueMovimento.InventarioItemId
                                        && x.Quantidade == estoqueMovimento.Quantidade
                                        && x.ProdutoId == estoqueMovimento.ProdutoId);

                                    if (item != null && !item.EstoqueMovimentosLoteValidades.IsNullOrEmpty())
                                    {
                                        item.Id = estoqueMovimento.Id;
                                        foreach (var estoqueMovimentosLoteValidade in item.EstoqueMovimentosLoteValidades)
                                        {
                                            estoqueMovimentosLoteValidade.EstoqueMovimentoItemId = item.Id;
                                        }

                                        estoqueMovimentosLoteValidades.AddRange(item.EstoqueMovimentosLoteValidades);
                                    }
                                }

                                QueryHelper.BulkData<EstoqueMovimentoLoteValidade>()
                                   .UserId(AbpSession.UserId)
                                   .AutoCloseTransaction(false)
                                   .Transaction(transaction)
                                   .BulkMerge(connection, estoqueMovimentosLoteValidades);


                                listaItems.ForEach(x => x.StatusInventarioItemId = StatusInventarioItem.Fechado);

                                QueryHelper.BulkData<InventarioItem>()
                                    .IgnoreField(x => x.UsuarioContegem)
                                    .IgnoreField(x => x.UsuarioContegem2)
                                    .IgnoreField(x => x.UsuarioContegem3)
                                    .IgnoreField(x => x.UsuarioDivergencia)
                                    .UserId(AbpSession.UserId)
                                    .AutoCloseTransaction(false)
                                    .Transaction(transaction)
                                    .BulkMerge(connection, listaItems);

                                QueryHelper.BulkData<ProdutoSaldo>()
                                    .UserId(AbpSession.UserId)
                                    .AutoCloseTransaction(false)
                                    .Transaction(transaction)
                                    .BulkMerge(connection, produtosAtualizarInserir);

                                transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                    }

                    if (defaultReturn.Errors.Count == 0)
                    {
                        inventario.StatusInventarioId = StatusInventario.Fechado;

                        using (var inventarioRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Inventario, long>>())
                        {
                            inventarioRepository.Object.Update(inventario);
                        }
                    }

                    unitOfWork.Complete();
                    unitOfWorkManager.Object.Current.SaveChanges();
                    produtoSaldoRepository.Object.GetDbContext().Configuration.AutoDetectChangesEnabled = true;
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                using (var produtoSaldoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoSaldo, long>>())
                {
                    produtoSaldoRepository.Object.GetDbContext().Configuration.AutoDetectChangesEnabled = true;
                    //produtoSaldoRepository.Object.GetDbContext().Configuration.ValidateOnSaveEnabled = true;
                }
                if (ex.InnerException != null)
                {
                    var inner = ex.InnerException;
                    defaultReturn.Errors.Add(ErroDto.Criar(inner.HResult.ToString(), inner.Message));
                }
                else
                {
                    defaultReturn.Errors.Add(ErroDto.Criar(ex.HResult.ToString(), ex.Message));
                }
            }

            return defaultReturn;
        }

        private static (EstoquePreMovimento, IEnumerable<EstoquePreMovimentoItem>, IEnumerable<ProdutoSaldo>) FechaInventarioPorMovimentacao(
            EstoquePreMovimento estoquePreMovimento,
            Inventario inventario,
            List<InventarioItem> lista,
            IEnumerable<ProdutoUnidade> unidades,
            IEnumerable<ProdutoSaldo> produtosSaldos)
        {
            var produtoSaldoResultado = new List<ProdutoSaldo>();
            var estoquePreMovimentoItemResultado = new List<EstoquePreMovimentoItem>();
            foreach (var item in lista)
            {
                var estoquePreMovimentoItem = new EstoquePreMovimentoItem
                {
                    Quantidade = (decimal)((item.QuantidadeContagem ?? 0) - (item.QuantidadeEstoque ?? 0)),
                    ProdutoId = item.ProdutoId,
                    InventarioItemId = item.Id
                };
                ProdutoSaldo produtoSaldo;
                if (item.LoteValidadeId != null)
                {
                    produtoSaldo = produtosSaldos.Where(w => w.ProdutoId == item.ProdutoId && w.LoteValidadeId == item.LoteValidadeId).FirstOrDefault();
                    estoquePreMovimentoItem.EstoquePreMovimentosLoteValidades = new List<EstoquePreMovimentoLoteValidade>
                    {
                        new EstoquePreMovimentoLoteValidade
                        {
                            LoteValidadeId = item.LoteValidadeId ?? 0,
                            Quantidade = (decimal)((item.QuantidadeContagem ?? 0) - (item.QuantidadeEstoque ?? 0))
                        }
                    };
                }
                else
                {
                    estoquePreMovimentoItem.Quantidade = (decimal)((item.QuantidadeContagem ?? 0) - (item.QuantidadeEstoque ?? 0));
                    produtoSaldo = produtosSaldos.Where(w => w.ProdutoId == item.ProdutoId && w.LoteValidadeId == null).FirstOrDefault();
                }

                var unidade = unidades.Where(w => w.ProdutoId == item.ProdutoId).FirstOrDefault();
                if (unidade != null)
                {
                    estoquePreMovimentoItem.ProdutoUnidadeId = unidade.UnidadeId;
                }

                estoquePreMovimentoItemResultado.Add(estoquePreMovimentoItem);

                if (produtoSaldo != null)
                {
                    produtoSaldo.QuantidadeAtual = item.QuantidadeContagem ?? 0;
                }
                else
                {
                    produtoSaldo = new ProdutoSaldo
                    {
                        ProdutoId = item.ProdutoId,
                        EstoqueId = inventario.EstoqueId,
                        LoteValidadeId = item.LoteValidadeId,
                        QuantidadeAtual = item.QuantidadeContagem ?? 0
                    };
                }

                produtoSaldoResultado.Add(produtoSaldo);
            }
            return (estoquePreMovimento, estoquePreMovimentoItemResultado, produtoSaldoResultado);
        }

        private async Task<long?> ObterEmpresaPrincipalEstoque(long estoqueId)
        {

            using (var estoqueEmpresaRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoqueEmpresa, long>>())
            {
                var estoqueEmpresa = await estoqueEmpresaRepository.Object.GetAll().AsNoTracking()
                                                  .Where(w => w.EstoqueId == estoqueId && w.IsPrincipal)
                                                  .FirstOrDefaultAsync();


                return estoqueEmpresa != null ? estoqueEmpresa.EmpresaId : (long?)0;
            }
        }

        private async Task<DefaultReturn<EstoquePreMovimentoItemDto>> InserirItem(InventarioItem item, long preMovimentoId, bool isEntrada)
        {
            using (var produtoUnidadeRepository = IocManager.Instance.ResolveAsDisposable<IRepository<ProdutoUnidade, long>>())
            using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
            using (var estoqueLoteValidadeAppService = IocManager.Instance.ResolveAsDisposable<IEstoqueLoteValidadeAppService>())
            {
                var estoquePreMovimentoItemDto = new EstoquePreMovimentoItemDto
                {
                    PreMovimentoId = preMovimentoId,// returnPreMovimentoEntrada.Id;
                    Quantidade = isEntrada ? (decimal)((item.QuantidadeContagem ?? 0) - (item.QuantidadeEstoque ?? 0)) : (decimal)((item.QuantidadeEstoque ?? 0) - (item.QuantidadeContagem ?? 0)),
                    ProdutoId = item.ProdutoId
                };

                var unidade = produtoUnidadeRepository.Object.GetAll()
                                                                .Where(w => w.ProdutoId == item.ProdutoId && w.UnidadeTipoId == 1)
                                                                .FirstOrDefault();
                if (unidade != null)
                {
                    estoquePreMovimentoItemDto.ProdutoUnidadeId = unidade.UnidadeId;
                }

                var preMovimentoItem = await estoquePreMovimentoItemAppService.Object.CriarOuEditar(estoquePreMovimentoItemDto);

                // defaultReturn.Errors.AddRange(preMovimentoItem.Errors);

                if (item.LoteValidadeId != null && preMovimentoItem.ReturnObject != null)
                {
                    var estoquePreMovimentoLoteValidadeDto = new EstoquePreMovimentoLoteValidadeDto
                    {
                        EstoquePreMovimentoItemId = preMovimentoItem.ReturnObject.Id,
                        LoteValidadeId = (long)item.LoteValidadeId,
                        Quantidade = estoquePreMovimentoItemDto.Quantidade
                    };

                    estoqueLoteValidadeAppService.Object.CriarOuEditar(estoquePreMovimentoLoteValidadeDto);
                }


                return preMovimentoItem;
            }
        }

        private static EstoqueMovimento GerarMovimento(EstoquePreMovimento preMovimento, IRepository<EstoqueMovimento, long> estoqueMovimentoRepository)
        {
            var estoqueMovimento = new EstoqueMovimento
            {
                EstoquePreMovimentoId = preMovimento.Id,
                Documento = preMovimento.Codigo,
                Movimento = preMovimento.Movimento,
                Emissao = preMovimento.Emissao,
                IsEntrada = preMovimento.IsEntrada,
                EstoqueId = preMovimento.EstoqueId,
                EstTipoMovimentoId = preMovimento.EstTipoMovimentoId,
                EmpresaId = preMovimento.EmpresaId,
                EstTipoOperacaoId = (long)EnumTipoOperacao.Inventario,
                // estoqueMovimento.InventarioId = id;
                PreMovimentoEstadoId = (long)EnumPreMovimentoEstado.Conferencia,
                GrupoOperacaoId = (long)EnumGrupoOperacao.Movimentos
            };

            estoqueMovimentoRepository.InsertAndGetId(estoqueMovimento);
            return estoqueMovimento;

        }

        private static IEnumerable<EstoqueMovimentoItem> GerarMovimentoItens(EstoqueMovimento estoqueMovimento, IEnumerable<EstoquePreMovimentoItem> preMovimentoItems)
        {
            var movimentoItens = new List<EstoqueMovimentoItem>();

            foreach (var item in preMovimentoItems)
            {
                var estoqueMovimentoItem = new EstoqueMovimentoItem
                {
                    EstoquePreMovimentoItemId = item.Id,
                    MovimentoId = estoqueMovimento.Id,
                    NumeroSerie = item.NumeroSerie,
                    ProdutoId = item.ProdutoId,
                    ProdutoUnidadeId = item.ProdutoUnidadeId,
                    Quantidade = item.Quantidade,
                    InventarioItemId = item.InventarioItemId
                };

                estoqueMovimentoItem.EstoqueMovimentosLoteValidades = new List<EstoqueMovimentoLoteValidade>();

                if (item.EstoquePreMovimentosLoteValidades != null)
                {
                    foreach (var itemLoteValidade in item.EstoquePreMovimentosLoteValidades)
                    {
                        var loteValidade = new EstoqueMovimentoLoteValidade
                        {
                            Quantidade = itemLoteValidade.Quantidade,
                            LoteValidadeId = itemLoteValidade.LoteValidadeId
                        };

                        estoqueMovimentoItem.EstoqueMovimentosLoteValidades.Add(loteValidade);
                    }
                }

                movimentoItens.Add(estoqueMovimentoItem);
            }

            return movimentoItens;
        }
    }
}
