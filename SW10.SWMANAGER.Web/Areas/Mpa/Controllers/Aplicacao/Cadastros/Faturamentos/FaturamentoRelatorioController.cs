using Abp.Web.Mvc.Authorization;
using Abp.Web.Security.AntiForgery;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamento.Relatorios;
using SW10.SWMANAGER.Web.Controllers;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Faturamentos
{
    [DisableAbpAntiForgeryTokenValidation]
    public class FaturamentoRelatorioController : SWMANAGERControllerBase
    {
        private IFaturamentoItemTabelaAppService _relatorioFaturamentoTabelaPrecoAppService;
        private ISessionAppService _sessionAppService;
        private readonly IUserAppService _userAppService;
        public FaturamentoRelatorioController(
            IFaturamentoItemTabelaAppService relatorioFaturamentoTabelaAppService,
            ISessionAppService sessionAppService,
            IUserAppService userAppService
            )
        {
            _relatorioFaturamentoTabelaPrecoAppService = relatorioFaturamentoTabelaAppService;
            _sessionAppService = sessionAppService;
            _userAppService = userAppService;
        }

        /// <summary>
        /// Entrada para filtro de visualização do Report de produtos
        /// </summary>
        /// <returns></returns>
        //GET: Mpa/Relatorios/SaldoProduto
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> Index(long id)
        {
            FaturamentoFiltroModel result = await CarregarIndex();

            result.TabelaId = id;
            //result.EhMovimentacao = false;
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Relatorios/Index.csHtml", result);
        }

        /// <summary>
        /// PartialView que renderiza o relatório com os filtros selecionados no formulário
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> Visualizar(string tabelaId)
        {
            ListarFaturamentoItensTabelaInput listarFaturamentoItensTabelaInput = new ListarFaturamentoItensTabelaInput();
            listarFaturamentoItensTabelaInput.TabelaId = tabelaId;
            listarFaturamentoItensTabelaInput.Sorting = "Descricao";
            var relatorioFaturamentoTabelaPreco = await _relatorioFaturamentoTabelaPrecoAppService.ListarParaFatTabela(listarFaturamentoItensTabelaInput);

            var loginInformations = await _sessionAppService.GetCurrentLoginInformations();

            //ListResultDto<EmpresaDto> empresaDto = new ListResultDto<EmpresaDto>();
            //var empresaDto = await _userAppService.GetUserEmpresas(loginInformations.User.Id);
            var userEmpresas = await _userAppService.GetUserEmpresas(loginInformations.User.Id);

            FaturamentoFiltroModel _filtro = new FaturamentoFiltroModel();
            _filtro.NomeHospital = userEmpresas.Items[0].NomeFantasia;

            if (relatorioFaturamentoTabelaPreco.Items.Count != 0)
            {
                _filtro.Titulo = string.Concat("Tabela de Preços - ", Convert.ToString(DateTime.Now));
                // _filtro.NomeHospital = "Lipp";
                // _filtro.NomeHospital = relatorioFaturamentoTabelaPreco.Items[0]..NomeFantasia.ToString();
                _filtro.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                _filtro.DataHora = Convert.ToString(DateTime.Now);

                _filtro.Lista = relatorioFaturamentoTabelaPreco.Items.Select(m => new CamposRelatorioDs
                {

                    codigo = m.Codigo,
                    descricao = m.Descricao == null ? string.Empty : Convert.ToString(m.Descricao),
                    moeda = m.SisMoeda.Descricao == null ? string.Empty : m.SisMoeda.Descricao,
                    tabela = m.Tabela == null ? string.Empty : m.Tabela.Descricao.ToString(),
                    dataVigencia = m.VigenciaDataInicio.ToString() == null ? string.Empty : string.Format("{0:dd/MM/yyyy}", m.VigenciaDataInicio),
                    preco = m.Preco.ToString() == null ? string.Empty : string.Format("{0:#,0.00}", m.Preco),
                    filme = m.Filme == null ? string.Empty : string.Format("{0:#,0.00}", m.Filme),
                    valoTotal = m.ValorTotal == null ? string.Empty : string.Format("{0:#,0.00}", m.ValorTotal),
                    auxilia = m.Auxiliar == null ? string.Empty : string.Format("{0:#,0.00}", m.Auxiliar),
                    porte = m.Porte == null ? string.Empty : string.Format("{0:#,0.00}", m.Porte),
                    ativo = m.IsAtivo.ToString() == null ? string.Empty : m.IsAtivo.ToString(),
                    item = m.Item == null ? string.Empty : m.Item.Descricao.ToString()
                }
                ).ToList();
            }
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/Relatorios/RelatorioTabelaPreco.aspx", _filtro);
        }


        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        //public ActionResult Exportar(FiltroModel filtro, string formato)
        //{
        //    if (formato != "Word" && formato != "Excel" && formato != "PDF")
        //    {
        //        throw new Exception();
        //    }

        //    ReportDataSource rd;
        //    LocalReport report;
        //    string mimeType;
        //    string encoding;
        //    string filenameExtension;
        //    string[] streams;
        //    Warning[] warnings;

        //    if (filtro.EhMovimentacao)
        //    {
        //        ProcessarDadosMovimentacao(filtro);
        //        report = new LocalReport()
        //        {
        //            ReportPath = "Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/Movimentacao.rdlc"
        //        };
        //        rd = new ReportDataSource("DataSet1", filtro.DadosMovimentacao);
        //    }
        //    else
        //    {
        //        ProcessarDadosProduto(filtro);
        //        report = new LocalReport()
        //        {
        //            ReportPath = "Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/ExtratoProdutos.rdlc"
        //        };
        //        rd = new ReportDataSource("DataSet1", filtro.Dados);
        //    }

        //    report.DataSources.Add(rd);
        //    var bytes = report.Render(formato, "", out mimeType, out encoding, out filenameExtension, out streams, out warnings);
        //    string extensionType = string.Empty;
        //    switch (formato)
        //    {
        //        case "Word":
        //            extensionType = ".doc";
        //            break;
        //        case "Excel":
        //            extensionType = ".xls";
        //            break;
        //        default:
        //            extensionType = ".pdf";
        //            break;
        //    }
        //    string fileName = string.Concat("RELATORIO", extensionType);
        //    return File(bytes, mimeType, fileName);
        //}

        ///// <summary>
        ///// Listar GrupoClasse com base do Grupo Selecionado
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        //public async Task<JsonResult> ListarGrupoClasse(int id)
        //{
        //    var result = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Selecione" } };

        //    result.AddRange((await IRelatorioSuprimentoAppService.Listar(new Grupo { Id = id }))
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.Id.ToString(),
        //            Text = s.Nome
        //        }));

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Suprimentos_Relatorio_SaldoProduto, AppPermissions.Pages_Tenant_Suprimentos_Relatorio_MovimentacaoProduto)]
        //public async Task<JsonResult> ListarGrupoSubClasse(int id)
        //{
        //    var result = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "Selecione" } };

        //    result.AddRange((await IRelatorioSuprimentoAppService.Listar(new GrupoClasse { Id = id }))
        //        .Select(s => new SelectListItem
        //        {
        //            Value = s.Id.ToString(),
        //            Text = s.Nome
        //        }));

        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        private async Task<FaturamentoFiltroModel> CarregarIndex()
        {
            var loginInformations = await _sessionAppService.GetCurrentLoginInformations();

            var userId = AbpSession.UserId;
            //var userEmpresas = _userAppService.GetUserEmpresas(userId.Value);
            // var userEmpresas = _relatorioAtendimentoAppService.ListarEmpresaUsuario(userId.Value);

            FaturamentoFiltroModel result = new FaturamentoFiltroModel();

            //result.Empresas = (_relatorioFaturamentoTabelaPrecoAppService.ListarEmpresaUsuario(userId.Value))
            //    .Select(s => new SelectListItem
            //    {
            //        Value = s.Id.ToString(),
            //        Text = s.Nome
            //    }).ToList();

            var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
            // result.Empresas.Insert(0, padrao);
            return result;
        }

        //private void ProcessarDadosProduto(FiltroModel filtro)
        //{
        //    var db = new Core.DataSetReportsTableAdapters.ReportProdutosTableAdapter();

        //    var grupo = filtro.GrupoProduto.GetValueOrDefault();
        //    var classe = filtro.Classe.GetValueOrDefault();
        //    var subClasse = filtro.SubClasse.GetValueOrDefault();
        //    if (grupo != 0 && classe == 0 && subClasse == 0)
        //    {
        //        filtro.Dados = db.GetDataByGrupoId(grupo).ToList();
        //    }
        //    else if (grupo != 0 && classe != 0 && subClasse == 0)
        //    {
        //        filtro.Dados = db.GetDataByGrupoClasseId(grupo, classe).ToList();
        //    }
        //    else if (grupo != 0 && classe != 0 && subClasse != 0)
        //    {
        //        filtro.Dados = db.GetDataBy(grupo, classe, subClasse).ToList();
        //    }
        //    else
        //    {
        //        filtro.Dados = db.GetData().ToList();
        //    }
        //}

        private void ProcessarDadosInternacao(FiltroModel filtro)
        {
            var db = new Core.DataSetReportsTableAdapters.RelatorioMovimentoAdapter();

            var grupo = filtro.GrupoProduto.GetValueOrDefault();
            var classe = filtro.Classe.GetValueOrDefault();
            var subClasse = filtro.SubClasse.GetValueOrDefault();
            var query = db.GetData().Where(w => w.GrupoId == grupo);

            if (classe != 0)
            {
                query = query.Where(w => w.GrupoClasseId == classe);
            }

            if (subClasse != 0)
            {
                query = query.Where(w => w.GrupoSubClasseId == subClasse);
            }

            filtro.DadosMovimentacao = query.ToList();
        }
    }
}