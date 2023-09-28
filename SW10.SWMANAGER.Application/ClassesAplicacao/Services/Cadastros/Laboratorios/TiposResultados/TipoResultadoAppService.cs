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
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados
{
    public class TipoResultadoAppService : SWMANAGERAppServiceBase, ITipoResultadoAppService
    {

        private readonly IListarTipoResultadosExcelExporter _listarTipoResultadosExcelExporter;
        private readonly IRepository<TipoResultado, long> _tipoResultadoRepositorio;


        public TipoResultadoAppService(IRepository<TipoResultado, long> tipoResultadoRepositorio, IListarTipoResultadosExcelExporter listarTipoResultadosExcelExporter)
        {
            _tipoResultadoRepositorio = tipoResultadoRepositorio;
            _listarTipoResultadosExcelExporter = listarTipoResultadosExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(TipoResultadoDto input)
        {
            try
            {
                var tipoResultado = input.MapTo<TipoResultado>();
                if (input.Id.Equals(0))
                {
                    await _tipoResultadoRepositorio.InsertOrUpdateAsync(tipoResultado);
                }
                else
                {
                    var ori = await _tipoResultadoRepositorio.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.IsSistema = input.IsSistema;

                    await _tipoResultadoRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TipoResultadoDto input)
        {
            try
            {
                await _tipoResultadoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<TipoResultado>> ListarTodos()
        {
            try
            {
                var query = await _tipoResultadoRepositorio
                    .GetAllListAsync();

                var tipoResultadosDto = query.MapTo<List<TipoResultado>>();

                return new ListResultDto<TipoResultado> { Items = tipoResultadosDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TipoResultadoDto>> Listar(ListarTipoResultadosInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<TipoResultado> tipoResultados;
            List<TipoResultadoDto> tipoResultadosDtos = new List<TipoResultadoDto>();
            try
            {
                var query = _tipoResultadoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                tipoResultados = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tipoResultadosDtos = TipoResultadoDto.Mapear(tipoResultados).ToList();

                return new PagedResultDto<TipoResultadoDto>(
                    contarTiposTabelaDominio,
                    tipoResultadosDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }


        public async Task<TipoResultadoDto> Obter(long id)
        {
            try
            {
                var result = await _tipoResultadoRepositorio.GetAsync(id);
                var tipoResultado = TipoResultadoDto.Mapear(result);
                return tipoResultado;
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
                var query = await _tipoResultadoRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var TipoResultados = new ListResultDto<GenericoIdNome> { Items = query };

                List<TipoResultadoDto> TipoResultadosList = new List<TipoResultadoDto>();

                return TipoResultados;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTipoResultadosInput input)
        {
            try
            {
                var result = await Listar(input);
                var TipoResultados = result.Items;
                return _listarTipoResultadosExcelExporter.ExportToFile(TipoResultados.ToList());
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
            List<TipoResultadoDto> pacientesDtos = new List<TipoResultadoDto>();
            try
            {
                //get com filtro
                var query = from p in _tipoResultadoRepositorio.GetAll()
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

