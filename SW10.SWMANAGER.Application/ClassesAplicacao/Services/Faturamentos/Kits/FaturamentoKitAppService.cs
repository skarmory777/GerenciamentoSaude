#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Repositorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Exporting;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.Sessions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using Dapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.Helpers;
using Abp.Dependency;
using Abp.Collections.Extensions;

#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits
{
    public class FaturamentoKitAppService : SWMANAGERAppServiceBase, IFaturamentoKitAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoKit, long> _kitRepository;
        private readonly IListarKitsExcelExporter _listarKitsExcelExporter;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ISessionAppService _sessionService;


        public FaturamentoKitAppService(
            IRepository<FaturamentoKit, long> kitRepository,
            IListarKitsExcelExporter listarKitsExcelExporter,
            IUnitOfWorkManager unitOfWorkManager,
            ISessionAppService sessionService
            )
        {
            _kitRepository = kitRepository;
            _listarKitsExcelExporter = listarKitsExcelExporter;
            _unitOfWorkManager = unitOfWorkManager;
            _sessionService = sessionService;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoKitDto>> Listar(ListarFaturamentoKitsInput input)
        {
            var itemrKits = 0;
            List<FaturamentoKit> itens;
            List<FaturamentoKitDto> itensDtos = new List<FaturamentoKitDto>();
            try
            {
                var query = _kitRepository
                    .GetAll().WhereIf(!string.IsNullOrEmpty(input.Filtro),
                    x => x.Descricao.Contains(input.Filtro));

                itemrKits = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync().ConfigureAwait(false);

                itensDtos = itens
                    .MapTo<List<FaturamentoKitDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoKitDto>(
                itemrKits,
                itensDtos
                );
        }


        public async Task CriarOuEditar(FaturamentoKitDto input)
        {
            try
            {
                using (var unitOfWork = _unitOfWorkManager.Begin())
                using (var fatKitItemRepository = IocManager.Instance.ResolveAsDisposable<IRepository<FaturamentoKitItem, long>>())
                {

                    var kit = FaturamentoKitDto.Mapear(input);

                    var itensQuantidades = JsonConvert.DeserializeObject<List<FaturamentoItemQuantidade>>(input.StrItensQtds);


                    if (input.Id == 0)
                    {
                        kit.FatItens = new List<FaturamentoKitItem>();

                        foreach (var item in itensQuantidades)
                        {
                            kit.FatItens.Add(new FaturamentoKitItem { FatItemId = item.ItemId, Quantidade = item.Quantidade });
                        }

                        _kitRepository.Insert(kit);
                    }
                    else
                    {
                        var IdsItensSalvos = fatKitItemRepository.Object.GetAll().Where(x => x.FatKitId == input.Id).Select(x => x.FatItemId).ToList();
                        var IdsItensRecebidos = itensQuantidades.Select(x => x.ItemId).ToList();
                        var IdsAExcluir = IdsItensSalvos.Where(x => !IdsItensRecebidos.Any(y => y == x));

                        foreach (var idItem in IdsAExcluir)
                        {
                            await fatKitItemRepository.Object.DeleteAsync(x =>
                            x.FatKitId == input.Id && x.FatItemId == idItem);
                        }


                        kit = await _kitRepository.GetAllIncluding(x => x.FatItens).SingleOrDefaultAsync(x => x.Id == input.Id);
                        kit.Observacao = input.Observacao;
                        kit.Codigo = input.Codigo;
                        kit.Descricao = input.Descricao;

                        foreach (var item in itensQuantidades)
                        {
                            if (kit.FatItens.Any(x => x.FatItemId == item.ItemId))
                                kit.FatItens.FirstOrDefault(x => x.FatItemId == item.ItemId).Quantidade = item.Quantidade;
                            else
                                kit.FatItens.Add(new FaturamentoKitItem { FatItemId = item.ItemId, Quantidade = item.Quantidade });
                        }

                        _kitRepository.Update(kit);
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


        //public async Task CriarOuEditar(FaturamentoKitDto input)
        //{
        //    try
        //    {
        //        var Kit = input.MapTo<FaturamentoKit>();

        //        if (input.Id.Equals(0))
        //        {
        //            var itens = input.itensIds.Distinct();

        //            using (var contexto = new SWMANAGERDbContext())
        //            {
        //                contexto.Entry(Kit).State = EntityState.Added;

        //                foreach (var itemId in itens)
        //                {
        //                    var itemDto = AsyncHelper.RunSync(() => _faturamentoItemAppService.Obter(itemId));
        //                    var item = itemDto.MapTo<FaturamentoItem>();

        //                    //  if (contexto.Entry(item).State == EntityState.Detached)
        //                    //contexto.FaturamentoItens.Attach(item);

        //                  //  Kit.Itens.Add(item);

        //                    contexto.Entry(item).State = EntityState.Modified;
        //                }

        //                contexto.SaveChanges();
        //            }
        //        }
        //        else
        //        {
        //            var itens = input.itensIds.Distinct();

        //            foreach (var itemId in itens)
        //            {
        //                var itemDto = AsyncHelper.RunSync(() => _faturamentoItemAppService.Obter(itemId));
        //                var item = itemDto.MapTo<FaturamentoItem>();
        //               // Kit.FatItens.Add(item);
        //            }

        //            //  using (var contexto = new SWMANAGERDbContext())
        //            // {
        //            var kitExistente = _kitRepository.GetAll()
        //                                              .Include(i => i.FatItens)
        //                                              .Where(s => s.Id == Kit.Id)
        //                                              .FirstOrDefault<FaturamentoKit>();

        //            if (kitExistente != null)
        //            {
        //                //Excluir
        //                kitExistente.FatItens.RemoveRange(0, kitExistente.FatItens.Count());



        //                //inclui
        //                foreach (var item in Kit.Itens)
        //                {
        //                    kitExistente.Itens.Add(item);
        //                }






        //                ////atuliza 
        //                //foreach (var item in kitExistente.Itens)
        //                //{
        //                //    var novoItem = Kit.Itens.Where(w => w.Id == item.Id)
        //                //                                   .First();

        //                //    item.GrupoId = novoItem.GrupoId;
        //                //    item.SubGrupoId = novoItem.SubGrupoId;
        //                //    item.QtdFatura = novoItem.QtdFatura;
        //                //    item.

        //                //}


        //                //List<FaturamentoItem> itensDeletados = new List<FaturamentoItem>();

        //                //foreach (var it in kitExistente.Itens)
        //                //{
        //                //    if (Kit.Itens.FirstOrDefault(ite => ite.Id == it.Id) == null)
        //                //        itensDeletados.Add(it);
        //                //}

        //                //List<FaturamentoItem> itensAdicionados = new List<FaturamentoItem>();

        //                //foreach (var it in Kit.Itens)
        //                //{
        //                //    if (kitExistente.Itens.FirstOrDefault(ite => ite.Id == it.Id) == null)
        //                //        itensAdicionados.Add(it);
        //                //}

        //                //itensDeletados.ForEach(c => kitExistente.Itens.Remove(c));

        //                //foreach (var c in itensAdicionados)
        //                //{
        //                //    // if (contexto.Entry(c).State == EntityState.Detached)
        //                //    //    contexto.FaturamentoItens.Attach(c);

        //                //    //   contexto.Entry(c).State = EntityState.Modified;

        //                //    kitExistente.Itens.Add(c);

        //                //    contexto.Entry(c).State = EntityState.Modified;
        //                //}

        //                kitExistente.Codigo = input.Codigo;
        //                kitExistente.Descricao = input.Descricao;

        //                await _kitRepository.UpdateAsync(kitExistente);
        //            }
        //            //  }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new UserFriendlyException(L("ErroSalvar"), ex);
        //    }
        //}

        public async Task Excluir(FaturamentoKitDto input)
        {
            try
            {
                await _kitRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoKitDto> Obter(long id)
        {
            try
            {
                var query = await _kitRepository
                    .GetAll()
                    .Include(m => m.FatItens)
                    .Include(m => m.FatItens.Select(mx => mx.FatItem))
                    .Where(m => m.Id == id).AsNoTracking()
                    .FirstOrDefaultAsync();

                var item = FaturamentoKitDto.Mapear(query);

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoKitDto> ObterDapper(long id)
        {
            try
            {


                using (var sqlConn = new SqlConnection(this.GetConnection()))
                {
                    var queryKit = $@"
                    SELECT 
                        {QueryHelper.CreateQueryFields<FaturamentoKit>("FatKit").AllowAllFields().GetFields()}
                    FROM
                        FatKit
                    WHERE 
                        FatKit.Id = @id AND FatKit.IsDeleted = @isDeleted;";
                    var queryKitItems = $@"
                    SELECT DISTINCT
                        {QueryHelper.CreateQueryFields<FaturamentoKitItem>("FatKitItem").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoItem>("FatItem").AllowAllFields().GetFields()},
                        {QueryHelper.CreateQueryFields<FaturamentoGrupo>("FatGrupo").AllowAllFields().GetFields()}
                    FROM FatKitItem
                        LEFT JOIN FatItem ON FatKitItem.FatItemId = FatItem.Id
                        LEFT JOIN FatGrupo ON FatItem.GrupoId = FatGrupo.Id
                    WHERE
                        FatKitItem.FatKitId = @id AND FatKitItem.IsDeleted = @isDeleted AND FatItem.IsDeleted = @isDeleted AND FatGrupo.IsDeleted = @isDeleted";

                    var result =
                        await sqlConn.QueryFirstOrDefaultAsync<FaturamentoKitDto>(queryKit,
                            new { id = id, isDeleted = false });
                    if (result == null)
                    {
                        return result;
                    }

                    result.Itens =
                        (await sqlConn.QueryAsync<FaturamentoKitItemDto, FaturamentoItemDto, FaturamentoGrupoDto, FaturamentoKitItemDto>(queryKitItems, ObterDapperMap, new { id = id, isDeleted = false })).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        private static FaturamentoKitItemDto ObterDapperMap(FaturamentoKitItemDto dto, FaturamentoItemDto itemDto, FaturamentoGrupoDto grupoDto)
        {
            if (dto == null)
            {
                return null;
            }

            if (itemDto != null)
            {
                itemDto.Grupo = grupoDto;
            }
            dto.FatItem = itemDto;
            return dto;
        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoKitsInput input)
        {
            try
            {
                var result = await Listar(input);
                var itens = result.Items;
                return _listarKitsExcelExporter.ExportToFile(itens.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ListResultDto<FaturamentoKitDto>> ListarTodos()
        {
            try
            {
                var query = _kitRepository.GetAll();

                var faturamentoKitsDto = await query.ToListAsync();

                return new ListResultDto<FaturamentoKitDto> { Items = faturamentoKitsDto.MapTo<List<FaturamentoKitDto>>() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<FaturamentoKitDto>> ListarPrevio(FaturamentoItemDto[] itens)
        {
            try
            {
                //    var query = _itemRepository.GetAll();

                //    var faturamentoKitsDto = await query.ToListAsync();

                return new ListResultDto<FaturamentoKitDto> { Items = itens.MapTo<List<FaturamentoKitDto>>() };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FaturamentoItemDto>> ListarPrevio(ListarItensKitFaturamentoInput input)
        {
            try
            {
                List<FaturamentoItemDto> listItens = new List<FaturamentoItemDto>();

                var kit = await Obter(input.KitFaturamentoId ?? 0);

                List<long> faturamentoItensIds = new List<long>();

                faturamentoItensIds = kit.Itens.Select(s => s.Id).ToList();


                //foreach(var id in kit.itensIds)
                //{
                //    //var novoItem = _faturamentoItemAppService.Obter(id);
                //    var novoItem = AsyncHelper.RunSync(() => _faturamentoItemAppService.Obter(id));
                //    listItens.Add(novoItem);
                //}

                // return new ListResultDto<FaturamentoItemDto> { Items = listItens.MapTo<List<FaturamentoItemDto>>() };


                //  long kitId = (input.KitFaturamentoId != null)? (long)input.KitFaturamentoId : 0;

                SWRepository<FaturamentoItem> _faturamentoItemRepository = new SWRepository<FaturamentoItem>(AbpSession, _sessionService);

                var query = _faturamentoItemRepository.GetAll()
                                          .Where(m => faturamentoItensIds.Any(a => a == m.Id))
                                          .Include(i => i.Grupo)
                                          .Include(i => i.Grupo.TipoGrupo)
                                          .Include(i => i.SubGrupo)
                                          ;
                //.Select( s=> s.Itens);





                var itens = query
                   .AsNoTracking()
                   .OrderBy(input.Sorting)
                   .PageBy(input)
                .ToList();

                var itensDto = new List<FaturamentoItemDto>();   //  itens.MapTo<List<FaturamentoItemDto>>();

                foreach (var item in itens)
                {
                    itensDto.Add(FaturamentoItemDto.Mapear(item));
                }

                return new PagedResultDto<FaturamentoItemDto>(query.Count(), itensDto);



            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoItemDto> faturamentoItensDto = new List<FaturamentoItemDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _kitRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                            m.Descricao.ToLower().Contains(dropdownInput.search.ToLower()) ||
                            m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                            )
                                //.Where(f => f.IsLaudo.Equals(isLaudo))
                            orderby p.Descricao ascending
                            select new DropdownItems
                            {
                                id = p.Id,
                                text = string.Concat(p.Codigo, " - ", p.Descricao)
                            };

                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                var result = await queryResultPage.ToListAsync();

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = result, TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<IResultDropdownList<long>> ListarDropdownKitContaMedica(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<DropdownInput>()
                .EnableDistinct()
                .AddIdField("FatKit.Id")
                .AddTextField("CASE WHEN FatItem.Id IS NOT NULL THEN CONCAT(FatContaKit.Codigo, ' - ', FatItem.Descricao) ELSE CONCAT(FatKit.Codigo, ' - ', FatKit.Descricao) END")
                .AddFromClause("FatContaKit INNER JOIN FatKit ON FatKit.Id = FatContaKit.FatKitId AND FatKit.IsDeleted = 0 LEFT JOIN FatItem ON FatItem.Id = FatKit.FaturamentoItemId AND FatItem.IsDeleted = 0")
                .AddWhereClause(@"FatContaId = @filtro AND FatContaKit.IsDeleted = 0
                    AND (
                        @search IS NULL
	                    OR FatItem.Descricao like '%' + @search + '%'
                        OR FatItem.Descricao like '%' + @search + '%'
	                    OR  FatItem.Codigo like '%' + @search + '%'
	                    OR  FatItem.CodAmb like '%' + @search + '%'
	                    OR  FatItem.CodCbhpm like '%' + @search + '%'
	                    OR  FatItem.CodTuss like '%' + @search + '%'
	                    OR  FatItem.DescricaoTuss like '%' + @search + '%'
                    )")
                .AddOrderByClause("CASE WHEN FatItem.Id IS NOT NULL THEN CONCAT(FatContaKit.Codigo, ' - ', FatItem.Descricao) ELSE CONCAT(FatKit.Codigo, ' - ', FatKit.Descricao) END ASC")
                .ExecuteAsync(dropdownInput);
        }

        public async Task<IResultDropdownList<long>> ListarDropdownKitContaMedicaPorKit(DropdownInput dropdownInput)
        {
            return await this.CreateSelect2<DropdownInput>()
                .EnableDistinct()
                .AddIdField("FatContaKit.Id")
                .AddTextField(@"CONCAT('Codigo: ',FatContaKit.Id, ' - Inicio: ', format(FatContaKit.Data,'dd/MM/yyyy HH:mm:ss'))")
                .AddFromClause("FatContaKit INNER JOIN FatKit ON FatKit.Id = FatContaKit.FatKitId LEFT JOIN FatItem ON FatItem.Id = FatKit.FaturamentoItemId")
                .AddWhereClause(@"FatContaId like @contaId AND FatContaKit.FatKitId like @fatKitId 
                    AND (
                        @search IS NULL 
	                    OR FatItem.Descricao like '%' + @search + '%'
	                    OR  FatItem.Codigo like '%' + @search + '%'
	                    OR  FatItem.CodAmb like '%' + @search + '%'
	                    OR  FatItem.CodCbhpm like '%' + @search + '%'
	                    OR  FatItem.CodTuss like '%' + @search + '%'
	                    OR  FatItem.DescricaoTuss like '%' + @search + '%'
                    )")
                .AddWhereMethod((input, dapperParameters) =>
                {
                    var contaId = "0";
                    var fatKitId = "0";
                    if (!input.filtros[0].IsNullOrEmpty())
                    {
                        contaId = input.filtros[0];
                    }

                    if (!input.filtros[1].IsNullOrEmpty())
                    {
                        fatKitId = input.filtros[1];
                    }

                    dapperParameters.Add("contaId", contaId);
                    dapperParameters.Add("fatKitId", fatKitId);

                    return "";
                })
                .AddOrderByClause(@"CONCAT('Codigo: ',FatContaKit.Id, ' - Inicio: ', format(FatContaKit.Data,'dd/MM/yyyy HH:mm:ss')) ASC")
                .ExecuteAsync(dropdownInput);
        }
    }

    //public class teste
    //{
    //    public long[] ids { get; set; }
    //}
}
