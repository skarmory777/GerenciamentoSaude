using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Exporting;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados
{
    public class TabelaResultadoAppService : SWMANAGERAppServiceBase, ITabelaResultadoAppService
    {

        private readonly IListarTabelaResultadosExcelExporter _listarTabelaResultadosExcelExporter;
        private readonly IRepository<TabelaResultado, long> _tabelaResultadoRepositorio;


        public TabelaResultadoAppService(IRepository<TabelaResultado, long> tabelaResultadoRepositorio, IListarTabelaResultadosExcelExporter listarTabelaResultadosExcelExporter)
        {
            _tabelaResultadoRepositorio = tabelaResultadoRepositorio;
            _listarTabelaResultadosExcelExporter = listarTabelaResultadosExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task CriarOuEditar(TabelaResultadoDto input)
        {
            try
            {
                var tabelaResultado = TabelaResultadoDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    await _tabelaResultadoRepositorio.InsertOrUpdateAsync(tabelaResultado);
                }
                else
                {
                    var ori = await _tabelaResultadoRepositorio.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.IsAlterado = input.IsAlterado;
                    ori.IsSistema = input.IsSistema;
                    ori.TabelaId = input.TabelaId;

                    await _tabelaResultadoRepositorio.UpdateAsync(ori);
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TabelaResultadoDto input)
        {
            try
            {
                await _tabelaResultadoRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<TabelaResultado>> ListarTodos()
        {
            try
            {
                var query = await _tabelaResultadoRepositorio
                    .GetAllListAsync();


                return new ListResultDto<TabelaResultado> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TabelaResultadoDto>> Listar(ListarTabelaResultadosInput input)
        {
            long tabelaId = 0;
            var tabelaTest = long.TryParse(input.PrincipalId, out tabelaId);
            var contarTiposTabelaDominio = 0;
            List<TabelaResultado> tabelaResultados;
            List<TabelaResultadoDto> tabelaResultadosDtos = new List<TabelaResultadoDto>();
            try
            {
                var query = _tabelaResultadoRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    )
                    .WhereIf(tabelaId > 0, m =>
                        m.TabelaId == tabelaId
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                tabelaResultados = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tabelaResultadosDtos = TabelaResultadoDto.Mapear(tabelaResultados).ToList();

                return new PagedResultDto<TabelaResultadoDto>(
                    contarTiposTabelaDominio,
                    tabelaResultadosDtos
                    );

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<TabelaResultadoDto> Obter(long id)
        {
            try
            {
                var result = await _tabelaResultadoRepositorio.GetAsync(id);
                var tabelaResultado = TabelaResultadoDto.Mapear(result);
                return tabelaResultado;
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
                var query = await _tabelaResultadoRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var TabelaResultados = new ListResultDto<GenericoIdNome> { Items = query };

                List<TabelaResultadoDto> TabelaResultadosList = new List<TabelaResultadoDto>();

                return TabelaResultados;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTabelaResultadosInput input)
        {
            try
            {
                var result = await Listar(input);
                var TabelaResultados = result.Items;
                return _listarTabelaResultadosExcelExporter.ExportToFile(TabelaResultados.ToList());
            }
            catch (Exception)
            {
                throw new UserFriendlyException(L("ErroExportar"));
            }
        }

        public async Task<IResultDropdownList<long>> ListarDropdown(DropdownInput dropdownInput)
        {
            long tabelaId;

            long.TryParse(dropdownInput.filtro, out tabelaId);


            return await base.ListarDropdownLambda(dropdownInput
                                                   , _tabelaResultadoRepositorio
                                                   , w => ((TabelaResultado)w).TabelaId == tabelaId
                                                   , p => new DropdownItems { id = p.Id, text = string.Concat(p.Codigo.ToString(), " - ", p.Descricao) }
                                                   , o => o.Descricao
                                                      );



            //int pageInt = int.Parse(dropdownInput.page) - 1;
            //var numberOfObjectsPerPage = int.Parse(dropdownInput.totalPorPagina);
            //List<TabelaResultadoDto> pacientesDtos = new List<TabelaResultadoDto>();
            //try
            //{
            //    //get com filtro
            //    var query = from p in _tabelaResultadoRepositorio.GetAll()
            //            .WhereIf(!dropdownInput.search.IsNullOrEmpty(), m =>
            //            m.Codigo.ToString().ToLower().Contains(dropdownInput.search.ToLower())
            //            //||
            //            //  m.NomeCompleto.ToLower().Contains(dropdownInput.search.ToLower())
            //            )
            //                orderby p.Descricao ascending
            //                select new DropdownItems { id = p.Id, text = string.Concat(p.Codigo, " - ", p.Descricao) };
            //    //paginação 
            //    var queryResultPage = query
            //      .Skip(numberOfObjectsPerPage * pageInt)
            //      .Take(numberOfObjectsPerPage);

            //    int total = await query.CountAsync();

            //    return new ResultDropdownList() { Items = queryResultPage.ToList(), TotalCount = total };
            //}
            //catch (Exception ex)
            //{
            //    throw new UserFriendlyException(L("ErroPesquisar"), ex);
            //}
        }

        public async Task<PagedResultDto<TabelaResultadoDto>> ListarJson(List<TabelaResultadoDto> input)
        {
            try
            {
                var result = await Task.Run(() => new PagedResultDto<TabelaResultadoDto>(input.Count(), input));
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }
    }
}

