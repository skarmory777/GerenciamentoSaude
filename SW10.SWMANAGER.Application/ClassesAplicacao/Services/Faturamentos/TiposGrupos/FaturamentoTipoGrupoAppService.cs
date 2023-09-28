#region Usings
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TiposGrupo;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupos;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
#endregion usings.

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo
{
    public class FaturamentoTipoGrupoAppService : SWMANAGERAppServiceBase, IFaturamentoTipoGrupoAppService
    {
        #region Cabecalho
        private readonly IRepository<FaturamentoTipoGrupo, long> _itemRepository;
        private readonly IListarFaturamentoTiposGrupoExcelExporter _listarFaturamentoTiposGrupoExcelExporter;

        public FaturamentoTipoGrupoAppService(IRepository<FaturamentoTipoGrupo, long> itemRepository, IListarFaturamentoTiposGrupoExcelExporter listarFaturamentoTiposGrupoExcelExporter)
        {
            _itemRepository = itemRepository;
            _listarFaturamentoTiposGrupoExcelExporter = listarFaturamentoTiposGrupoExcelExporter;
        }
        #endregion cabecalho.

        public async Task<PagedResultDto<FaturamentoTipoGrupoDto>> Listar(ListarFaturamentoTiposGrupoInput input)
        {
            var itemrFaturamentoTiposGrupo = 0;
            List<FaturamentoTipoGrupo> itens;
            List<FaturamentoTipoGrupoDto> itensDtos = new List<FaturamentoTipoGrupoDto>();
            try
            {
                var query = _itemRepository
                    .GetAll()

                    //.Include(m => m.Estado)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    ;

                itemrFaturamentoTiposGrupo = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<FaturamentoTipoGrupoDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<FaturamentoTipoGrupoDto>(
                itemrFaturamentoTiposGrupo,
                itensDtos
                );
        }

        public async Task CriarOuEditar(FaturamentoTipoGrupoDto input)
        {
            try
            {
                var Item = input.MapTo<FaturamentoTipoGrupo>();
                if (input.Id.Equals(0))
                {
                    await _itemRepository.InsertAsync(Item);
                }
                else
                {
                    await _itemRepository.UpdateAsync(Item);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(FaturamentoTipoGrupoDto input)
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

        public async Task<FaturamentoTipoGrupoDto> Obter(long id)
        {
            try
            {
                var query = await _itemRepository
                    .GetAll()
                    //     .Include(m => m.Estado)
                    .Where(m => m.Id == id)
                    .FirstOrDefaultAsync();

                var item = query
                    .MapTo<FaturamentoTipoGrupoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FaturamentoTipoGrupoDto> ObterComEstado(string nome, long estadoId)
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
                    .MapTo<FaturamentoTipoGrupoDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarFaturamentoTiposGrupoInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarFaturamentoTiposGrupoExcelExporter.ExportToFile(itens.ToList());
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroExportar"));
            //}
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            int numberOfObjectsPerPage = 1;

            List<FaturamentoTipoGrupoDto> faturamentoItensDto = new List<FaturamentoTipoGrupoDto>();
            try
            {
                if (!int.TryParse(dropdownInput.totalPorPagina, out numberOfObjectsPerPage))
                {
                    throw new Exception("NotANumber");
                }

                //      bool isLaudo = (!dropdownInput.filtro.IsNullOrEmpty()) ? dropdownInput.filtro.Equals("IsLaudo") : false;

                var query = from p in _itemRepository.GetAll()
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
    }
}
