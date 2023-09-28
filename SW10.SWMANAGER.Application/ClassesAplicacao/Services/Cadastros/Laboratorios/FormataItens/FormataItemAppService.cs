using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Base.Dropdown;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems.Exporting;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.FormataItems
{
    public class FormataItemAppService : SWMANAGERAppServiceBase, IFormataItemAppService
    {

        private readonly IListarFormataItemsExcelExporter _listarFormataItemsExcelExporter;
        private readonly IRepository<FormataItem, long> _formataItemRepositorio;
        private readonly IRepository<ResultadoExame, long> _resultadoExameRepository;


        public FormataItemAppService(IRepository<FormataItem, long> formataItemRepositorio
                                   , IListarFormataItemsExcelExporter listarFormataItemsExcelExporter
                                   , IRepository<ResultadoExame, long> resultadoExameRepository)
        {
            _formataItemRepositorio = formataItemRepositorio;
            _listarFormataItemsExcelExporter = listarFormataItemsExcelExporter;
            _resultadoExameRepository = resultadoExameRepository;
        }

        [AbpAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_FormataItem_Edit)]
        public async Task<DefaultReturn<FormataItemDto>> CriarOuEditar(FormataItemDto input)
        {
            DefaultReturn<FormataItemDto> _retornoPadrao = new DefaultReturn<FormataItemDto>();
            _retornoPadrao.Warnings = new List<ErroDto>();

            try
            {
                FormataItemValidacaoService formataItemValidacaoService = new FormataItemValidacaoService(_formataItemRepositorio);
                _retornoPadrao.Errors = formataItemValidacaoService.Validar(input);

                if (_retornoPadrao.Errors.Count == 0)
                {

                    var formataItem = FormataItemDto.Mapear(input);
                    if (input.Id.Equals(0))
                    {
                        await _formataItemRepositorio.InsertOrUpdateAsync(formataItem);
                    }
                    else
                    {
                        var ori = await _formataItemRepositorio.GetAsync(input.Id);
                        ori.Codigo = input.Codigo;
                        ori.Descricao = input.Descricao;
                        ori.FormataId = input.FormataId;
                        ori.Formula = input.Formula;
                        ori.IsBI = input.IsBI;
                        ori.IsRefExame = input.IsRefExame;
                        ori.IsSistema = input.IsSistema;
                        ori.ItemResultadoId = input.ItemResultadoId;
                        //ori.LaboratorioUnidadeId = input.LaboratorioUnidadeId;
                        ori.Ordem = input.Ordem;
                        ori.OrdemRegistro = input.OrdemRegistro;
                        //ori.TipoResultadoId = input.TipoResultadoId;

                        await _formataItemRepositorio.UpdateAsync(ori);
                    }
                }
                _retornoPadrao.ReturnObject = input;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroSalvar"), ex);
            }
            return _retornoPadrao;
        }

        public async Task Excluir(FormataItemDto input)
        {
            try
            {
                await _formataItemRepositorio.DeleteAsync(input.Id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroExcluir"), ex);
            }

        }

        public async Task<ListResultDto<FormataItem>> ListarTodos()
        {
            try
            {
                var query = await _formataItemRepositorio
                    .GetAllListAsync();

                return new ListResultDto<FormataItem> { Items = query };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<PagedResultDto<FormataItemDto>> Listar(ListarFormataItemsInput input)
        {
            var contarTiposTabelaDominio = 0;
            List<FormataItem> formataItems;
            List<FormataItemDto> formataItemsDtos = new List<FormataItemDto>();
            try
            {
                var query = _formataItemRepositorio
                    .GetAll()
                    .Include(i => i.ItemResultado)
                    .Where(w => w.FormataId == input.FormataId);
                //.WhereIf(!input.Filtro.IsNullOrEmpty(), m =>
                //    m.Codigo.Contains(input.Filtro) ||
                //    m.Descricao.ToUpper().Contains(input.Filtro.ToUpper())
                ;// );

                contarTiposTabelaDominio = await query
                    .CountAsync();

                formataItems = await query
                    .AsNoTracking()
                    .OrderBy("Ordem")
                    .PageBy(input)
                    .ToListAsync();

                formataItemsDtos = FormataItemDto.Mapear(formataItems).ToList();

                return new PagedResultDto<FormataItemDto>(
                    contarTiposTabelaDominio,
                    formataItemsDtos
                    );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FormataItemDto> Obter(long id)
        {
            try
            {
                var query = _formataItemRepositorio
                    .GetAll()
                    .Include(i => i.ItemResultado)
                    .Include(i => i.ItemResultado.TipoResultado)
                    .Include(i => i.ItemResultado.LaboratorioUnidade)
                    .Where(w => w.Id == id);

                var formataItem = await query.FirstOrDefaultAsync();

                var formataItemDto = FormataItemDto.Mapear(formataItem);

                return formataItemDto;
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
                var query = await _formataItemRepositorio
                    .GetAll()
                    .WhereIf(!input.IsNullOrEmpty(), m =>
                        m.Descricao.ToUpper().Contains(input.ToUpper())
                    )
                    .Select(m => new GenericoIdNome { Id = m.Id, Nome = m.Descricao })
                    .ToListAsync();

                var FormataItems = new ListResultDto<GenericoIdNome> { Items = query };

                List<FormataItemDto> FormataItemsList = new List<FormataItemDto>();

                return FormataItems;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

        public async Task<FileDto> ListarParaExcel(ListarFormataItemsInput input)
        {
            try
            {
                var result = await Listar(input);
                var FormataItems = result.Items;
                return _listarFormataItemsExcelExporter.ExportToFile(FormataItems.ToList());
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
            List<FormataItemDto> pacientesDtos = new List<FormataItemDto>();
            try
            {
                //get com filtro
                var query = from p in _formataItemRepositorio.GetAll()
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

        public async Task<string> CalcularFormula(string input, long itemResultadoId)
        {
            List<FormatacaoItemIndexDto> resultadosDto = JsonConvert.DeserializeObject<List<FormatacaoItemIndexDto>>(input);

            var itemResultado = resultadosDto.Where(w => w.ItemId == itemResultadoId).FirstOrDefault();

            var resultadoExame = _resultadoExameRepository.GetAll()
                                                          .Where(w => w.Id == itemResultado.ResultadoExameId)
                                                          .FirstOrDefault();

            if (resultadoExame != null)
            {
                var formataItem = _formataItemRepositorio.GetAll()
                                                         .Where(w => w.FormataId == resultadoExame.FormataId
                                                                  && w.ItemResultadoId == itemResultadoId)
                                                                  .FirstOrDefault();

                Dictionary<string, string> codigos = new Dictionary<string, string>();

                if (formataItem != null)
                {
                    var formula = formataItem.Formula;

                    string pattern = Regex.Escape("[") + "(.*?)]";
                    MatchCollection matches = Regex.Matches(formula, pattern);

                    foreach (Match item in matches)
                    {

                        var resultado = resultadosDto.Where(w => w.CodigoItem == item.Groups[1].Value)
                                                     .FirstOrDefault()
                                                     .Resultado;


                        formula = formula.Replace(item.Value, resultado.Replace(',', '.'));
                        //  codigos.Add(item.Groups[1].Value, resultado);
                        //.Add(item.Groups[1].Value);
                    }

                    DataTable dt = new DataTable();
                    var v = dt.Compute(formula, "");

                }
            }



            return "";
        }

        public async Task<PagedResultDto<FormataItemDto>> ListarJson(List<FormataItemDto> input)
        {
            try
            {
                var result = await Task.Run(() => new PagedResultDto<FormataItemDto>(input.Count(), input));
                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(L("ErroPesquisar"), ex);
            }
        }

    }
}

