using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposSanguineos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos
{
    public class TipoSanguineoAppService : SWMANAGERAppServiceBase, ITipoSanguineoAppService
    {
        private readonly IRepository<TipoSanguineo, long> _TipoSanguineoRepository;
        private readonly IListarTiposSanguineosExcelExporter _listarTiposSanguineosExcelExporter;

        public TipoSanguineoAppService(IRepository<TipoSanguineo, long> TipoSanguineoRepository, IListarTiposSanguineosExcelExporter listarTiposSanguineosExcelExporter)
        {
            _TipoSanguineoRepository = TipoSanguineoRepository;
            _listarTiposSanguineosExcelExporter = listarTiposSanguineosExcelExporter;
        }

        public async Task CriarOuEditar(TipoSanguineoDto input)
        {
            try
            {
                var TipoSanguineo = input.MapTo<TipoSanguineo>();
                if (input.Id.Equals(0))
                {
                    await _TipoSanguineoRepository.InsertAsync(TipoSanguineo);
                }
                else
                {
                    var ori = await _TipoSanguineoRepository.GetAsync(input.Id);

                    ori.Codigo = input.Codigo;
                    ori.Codigo = input.Descricao;

                    await _TipoSanguineoRepository.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TipoSanguineoDto input)
        {
            try
            {
                await _TipoSanguineoRepository.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<PagedResultDto<TipoSanguineoDto>> Listar(ListarTiposSanguineosInput input)
        {
            var contarTiposSanguineos = 0;
            List<TipoSanguineo> TiposSanguineos;
            List<TipoSanguineoDto> TiposSanguineosDtos = new List<TipoSanguineoDto>();
            try
            {
                var query = _TipoSanguineoRepository
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposSanguineos = await query
                    .CountAsync();

                TiposSanguineos = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                TiposSanguineosDtos = TiposSanguineos
                    .MapTo<List<TipoSanguineoDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TipoSanguineoDto>(
                contarTiposSanguineos,
                TiposSanguineosDtos
                );
        }

        public async Task<ListResultDto<TipoSanguineoDto>> ListarTodos()
        {
            try
            {
                var TiposSanguineos = await _TipoSanguineoRepository
                    .GetAll()
                    .AsNoTracking()
                    .ToListAsync();

                var TiposSanguineosDtos = TiposSanguineos
                    .MapTo<List<TipoSanguineoDto>>();

                return new ListResultDto<TipoSanguineoDto> { Items = TiposSanguineosDtos };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTiposSanguineosInput input)
        {
            try
            {
                var result = await Listar(input);
                var TiposSanguineos = result.Items;
                return _listarTiposSanguineosExcelExporter.ExportToFile(TiposSanguineos.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<TipoSanguineoDto> Obter(long id)
        {
            try
            {
                var result = await _TipoSanguineoRepository
                    .GetAsync(id);

                var TipoSanguineo = result
                    //.FirstOrDefault()
                    .MapTo<TipoSanguineoDto>();

                return TipoSanguineo;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            //return await ListarCodigoDescricaoDropdown(dropdownInput, _TipoSanguineoRepository);
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            //List<TipoSanguineoDto> tipoSanguineoDtos = new List<TipoSanguineoDto>();

            try
            {
                //get com filtro
                var query = from p in _TipoSanguineoRepository.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Descricao.ToLower().Contains(dropdownInput.search.ToLower())

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
