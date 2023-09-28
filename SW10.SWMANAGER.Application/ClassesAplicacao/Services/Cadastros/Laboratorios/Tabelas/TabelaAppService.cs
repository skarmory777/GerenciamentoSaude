using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tabelas
{
    public class TabelaAppService : SWMANAGERAppServiceBase, ITabelaAppService
    {

        private readonly IListarTabelasExcelExporter _listarTabelasExcelExporter;
        private readonly IRepository<Tabela, long> _tabelaRepositorio;
        private readonly IRepository<TabelaResultado, long> _tabelaResultadoRepositorio;

        public TabelaAppService(
            IRepository<Tabela, long> tabelaRepositorio,
            IListarTabelasExcelExporter listarTabelasExcelExporter,
            IRepository<TabelaResultado, long> tabelaResultadoRepositorio
            )
        {
            _tabelaRepositorio = tabelaRepositorio;
            _tabelaResultadoRepositorio = tabelaResultadoRepositorio;
            _listarTabelasExcelExporter = listarTabelasExcelExporter;
        }
        [AbpAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tabela_Edit)]
        public async Task CriarOuEditar(TabelaDto input)
        {
            try
            {
                var tabela = TabelaDto.Mapear(input);
                if (input.Id.Equals(0))
                {
                    input.Id = await _tabelaRepositorio.InsertAndGetIdAsync(tabela);
                }
                else
                {
                    var ori = await _tabelaRepositorio.GetAsync(input.Id);
                    ori.Codigo = input.Codigo;
                    ori.Descricao = input.Descricao;
                    ori.IsSistema = input.IsSistema;

                    await _tabelaRepositorio.UpdateAsync(ori);
                }

                //Resultados
                if (!input.Resultados.IsNullOrWhiteSpace())
                {
                    var resultados = JsonConvert.DeserializeObject<List<TabelaResultado>>(input.Resultados, new IsoDateTimeConverter() { DateTimeFormat = "dd/MM/yyyy" });
                    foreach (var resultado in resultados)
                    {
                        if (resultado.TabelaId != input.Id)
                        {
                            resultado.TabelaId = input.Id;
                        }
                        resultado.Tabela = null;
                        if (resultado.Id > 0)
                        {
                            await _tabelaResultadoRepositorio.UpdateAsync(resultado);
                        }
                        else
                        {
                            await _tabelaResultadoRepositorio.InsertAsync(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }

        }

        public async Task Excluir(TabelaDto input)
        {
            try
            {
                await _tabelaRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<Tabela>> ListarTodos()
        {
            try
            {
                var query = await _tabelaRepositorio
                    .GetAllListAsync();

                var tabelasDto = query.MapTo<List<Tabela>>();

                return new ListResultDto<Tabela> { Items = tabelasDto };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<TabelaDto>> Listar(ListarTabelasInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<Tabela> tabelas;
            List<TabelaDto> tabelasDtos = new List<TabelaDto>();
            try
            {
                var query = _tabelaRepositorio
                    .GetAll()
                    .WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                        m.Codigo.Contains(input.Filtro) ||
                        m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                    );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                tabelas = await query
                    .AsNoTracking()
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                tabelasDtos = TabelaDto.Mapear(tabelas).ToList();

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
            return new PagedResultDto<TabelaDto>(
                contarTiposTabelaDominio,
                tabelasDtos
                );
        }

        public async Task<TabelaDto> Obter(long id)
        {
            try
            {
                var result = await _tabelaRepositorio.GetAsync(id);
                var tabela = TabelaDto.Mapear(result);
                //Resultados
                var idGrid = 1;
                var resultados = await _tabelaResultadoRepositorio
                    .GetAll()
                    .Where(m => m.TabelaId == id)
                    .ToListAsync();
                var resultadosDto = TabelaResultadoDto.Mapear(resultados).ToList();

                resultadosDto.ForEach(m => m.IdGrid = idGrid++);
                tabela.Resultados = JsonConvert.SerializeObject(resultadosDto.ToList());

                return tabela;
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
                var query = await _tabelaRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var Tabelas = new ListResultDto<GenericoIdNome> { Items = query };

                List<TabelaDto> TabelasList = new List<TabelaDto>();

                return Tabelas;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarTabelasInput input)
        {
            try
            {
                var result = await Listar(input);
                var Tabelas = result.Items;
                return _listarTabelasExcelExporter.ExportToFile(Tabelas.ToList());
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
            List<TabelaDto> pacientesDtos = new List<TabelaDto>();
            try
            {
                //get com filtro
                var query = from p in _tabelaRepositorio.GetAll()
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

