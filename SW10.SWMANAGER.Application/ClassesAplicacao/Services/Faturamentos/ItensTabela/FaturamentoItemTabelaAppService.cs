#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela
{
    public class FaturamentoItemTabelaAppService : SWMANAGERAppServiceBase, IFaturamentoItemTabelaAppService
    {
        #region Cabecalho

        private readonly IRepository<FaturamentoItemTabela, long> _itemRepository;
        //     private readonly IListarFaturamentoItensTabelaExcelExporter _listarItensTabelaExcelExporter;

        public FaturamentoItemTabelaAppService(
            IRepository<FaturamentoItemTabela, long> itemRepository
            //,
            //     IListarFaturamentoItensTabelaExcelExporter listarItensTabelaExcelExporter
            )
        {
            _itemRepository = itemRepository;
            //    _listarItensTabelaExcelExporter = listarItensTabelaExcelExporter;
        }

        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoItemTabelaDto>> Listar(ListarFaturamentoItensTabelaInput input)
        {
            var itemrItensTabela = 0;
            List<FaturamentoItemTabela> itens;
            List<FaturamentoItemTabelaDto> itensDtos = new List<FaturamentoItemTabelaDto>();
            try
            {
                var query = _itemRepository
                    .GetAll()
                    //  .Include(m => m.TipoItemTabela)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    //   .Where(i=>i.TabelaId.ToString() == input.Filtro)
                    ;

                itemrItensTabela = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoItemTabelaDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoItemTabelaDto>(
                itemrItensTabela,
                itensDtos
                );
        }

        public async Task<PagedResultDto<FaturamentoItemTabelaDto>> ListarParaFatItem(ListarFaturamentoItensTabelaInput input)
        {
            var itemrItensTabela = 0;
            List<FaturamentoItemTabela> itens;
            List<FaturamentoItemTabelaDto> itensDtos = new List<FaturamentoItemTabelaDto>();
            try
            {
                var query = _itemRepository
                    .GetAll()
                    .Include(m => m.Tabela)
                    .Include(m => m.Item)
                    .Include(m => m.SisMoeda)
                    .Where(i => i.ItemId.ToString() == input.ItemId)
                    .WhereIf(!input.MoedaId.IsNullOrEmpty(), i => i.SisMoedaId.ToString() == input.MoedaId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), i => i.Descricao.Contains(input.Filtro))
                    ;

                itemrItensTabela = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoItemTabelaDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoItemTabelaDto>(
                itemrItensTabela,
                itensDtos
                );
        }

        public async Task<PagedResultDto<FaturamentoItemTabelaDto>> ListarParaFatTabela(ListarFaturamentoItensTabelaInput input)
        {
            var itemrItensTabela = 0;
            List<FaturamentoItemTabela> itens;
            List<FaturamentoItemTabelaDto> itensDtos = new List<FaturamentoItemTabelaDto>();
            try
            {
                var query = _itemRepository
                    .GetAll()
                    .Include(m => m.Tabela)
                    .Include(m => m.Item)
                    .Include(m => m.SisMoeda)
                      //.WhereIf(!input.EstadoId.Equals(0), m =>
                      //    m.EstadoId == input.EstadoId
                      //)
                      .Where(i => i.TabelaId.ToString() == input.TabelaId)
                    .WhereIf(!input.MoedaId.IsNullOrEmpty(), i => i.SisMoedaId.ToString() == input.MoedaId)
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), i => i.Descricao.Contains(input.Filtro))
                    ;

                itemrItensTabela = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoItemTabelaDto>>();

                return new PagedResultDto<FaturamentoItemTabelaDto>(
                    itemrItensTabela,
                    itensDtos
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task CriarOuEditar(FaturamentoItemTabelaDto input)
        {
            try
            {
                var ItemTabela = input.MapTo<FaturamentoItemTabela>();
                if (input.Id.Equals(0))
                {
                    await _itemRepository.InsertAsync(ItemTabela);
                }
                else
                {
                    await _itemRepository.UpdateAsync(ItemTabela);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoItemTabelaDto input)
        {
            try
            {
                await _itemRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }
        }

        public async Task<FaturamentoItemTabelaDto> Obter(long id)
        {
            try
            {
                var query = await _itemRepository
                      .GetAll()
                      .Include(m => m.Item)
                      .Include(_ => _.SisMoeda)
                      .Include(_ => _.Tabela)
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoItemTabelaDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoItemTabelaDto> ObterComEstado(string nome, long estadoId)
        {
            try
            {
                var query = _itemRepository
                    .GetAll()
                    //.Include(m => m.Estado)
                    //.Where(m =>
                    //    m.Nome.ToUpper().Equals(nome.ToUpper()) &&
                    //    m.EstadoId.Equals(estadoId)
                    //)
                    ;

                var result = await query.FirstOrDefaultAsync();

                var item = result
                    .MapTo<FaturamentoItemTabelaDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoItensTabelaInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarItensTabelaExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            try
            {
                //get com filtro
                var query = from p in _itemRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //m.NomeFantasia.ToLower().Contains(dropdownInput.search.ToLower()
                        //)
                        )
                            orderby p.Descricao ascending
                            select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
                //paginação 
                var queryResultPage = query
                  .Skip(numberOfObjectsPerPage * pageInt)
                  .Take(numberOfObjectsPerPage);

                int total = await query.CountAsync();

                return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}
