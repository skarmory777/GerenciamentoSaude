using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;

using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.SisMoedas;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos
{
    public class SisMoedaAppService : SWMANAGERAppServiceBase, ISisMoedaAppService
    {
        private readonly IRepository<SisMoeda, long> _itemRepository;
        //     private readonly IListarSisMoedasExcelExporter _listarGruposExcelExporter;

        public SisMoedaAppService(
            IRepository<SisMoeda, long> itemRepository
            //,
            //     IListarSisMoedasExcelExporter listarGruposExcelExporter
            )
        {
            _itemRepository = itemRepository;
            //    _listarGruposExcelExporter = listarGruposExcelExporter;
        }


        public async Task<PagedResultDto<SisMoedaDto>> Listar(ListarSisMoedasInput input)
        {
            var itemrGrupos = 0;
            List<SisMoeda> itens;
            List<SisMoedaDto> itensDtos = new List<SisMoedaDto>();
            try
            {
                var query = _itemRepository
                    .GetAll()
                    //   .Include(m => m.TipoGrupo)
                    //.WhereIf(!input.EstadoId.Equals(0), m =>
                    //    m.EstadoId == input.EstadoId
                    //)
                    ;

                itemrGrupos = await query
                    .CountAsync();

                itens = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                itensDtos = itens
                    .MapTo<List<SisMoedaDto>>();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<SisMoedaDto>(
                itemrGrupos,
                itensDtos
                );
        }

        public async Task CriarOuEditar(SisMoedaDto input)
        {
            try
            {
                var Grupo = input.MapTo<SisMoeda>();
                if (input.Id.Equals(0))
                {
                    await _itemRepository.InsertAsync(Grupo);
                }
                else
                {
                    await _itemRepository.UpdateAsync(Grupo);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
        }

        public async Task Excluir(SisMoedaDto input)
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

        public async Task<SisMoedaDto> Obter(long id)
        {
            try
            {
                var query = await _itemRepository
                      .GetAll()
                      //  .Include(m => m.TipoGrupo)
                      .Where(m => m.Id == id)
                      .FirstOrDefaultAsync();

                var item = query
                    .MapTo<SisMoedaDto>();

                return item;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }

        }

        public async Task<FileDto> ListarParaExcel(ListarSisMoedasInput input)
        {
            return null;
            //try
            //{
            //    var result = await Listar(input);
            //    var itens = result.Items;
            //    return _listarGruposExcelExporter.ExportToFile(itens.ToList());
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
