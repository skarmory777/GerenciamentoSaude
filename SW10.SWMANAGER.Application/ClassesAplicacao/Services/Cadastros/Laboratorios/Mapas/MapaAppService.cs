using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas
{
    public class MapaAppService : SWMANAGERAppServiceBase, IMapaAppService
    {

        private readonly IListarMapasExcelExporter _listarMapasExcelExporter;
        private readonly IRepository<Mapa, long> _mapaRepositorio;


        public MapaAppService(IRepository<Mapa, long> mapaRepositorio, IListarMapasExcelExporter listarMapasExcelExporter)
        {
            _mapaRepositorio = mapaRepositorio;
            _listarMapasExcelExporter = listarMapasExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(MapaDto input)
        {
            try
            {
                var mapa = input.MapTo<Mapa>();
                if (input.Id.Equals(0))
                {
                    await _mapaRepositorio.InsertOrUpdateAsync(mapa);
                }
                else
                {
                    await _mapaRepositorio.UpdateAsync(mapa);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(MapaDto input)
        {
            try
            {
                await _mapaRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<Mapa>> ListarTodos()
        {
            try
            {
                var query = await _mapaRepositorio
                    .GetAllListAsync();

                var mapasDto = query.MapTo<List<Mapa>>();

                return new ListResultDto<Mapa> { Items = mapasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<MapaDto>> Listar(ListarMapasInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Mapa> mapas;
            List<MapaDto> mapasDtos = new List<MapaDto>();
            try
            {
                var query = _mapaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.Contains(input.Filtro)
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                mapas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                mapasDtos = mapas
                    .MapTo<List<MapaDto>>();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<MapaDto>(
                contarTiposTabelaDominio,
                mapasDtos
                );
        }


        public async Task<MapaDto> Obter(long id)
        {
            try
            {
                var result = await _mapaRepositorio.GetAsync(id);
                var mapa = result.MapTo<MapaDto>();
                return mapa;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<ListResultDto<GenericoIdNome>> ListarAutoComplete(string input)
        {
            try
            {
                var query = await _mapaRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.Contains(input)
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Mapas = new ListResultDto<GenericoIdNome> { Items = query };

                List<MapaDto> MapasList = new List<MapaDto>();

                return Mapas;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarMapasInput input)
        {
            try
            {
                var result = await Listar(input);
                var Mapas = result.Items;
                return _listarMapasExcelExporter.ExportToFile(Mapas.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<ResultDropdownList> ListarDropdown(DropdownInput dropdownInput)
        {
            int pageInt = int.Parse(dropdownInput.page) - 1;
            var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            List<MapaDto> pacientesDtos = new List<MapaDto>();
            try
            {
                //get com filtro
                var query = from p in _mapaRepositorio.GetAll()
                        .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
                        m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
                        //||
                        //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
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

