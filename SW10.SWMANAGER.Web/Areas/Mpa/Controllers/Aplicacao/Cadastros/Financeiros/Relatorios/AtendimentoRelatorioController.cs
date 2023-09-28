using Abp.Web.Mvc.Authorization;
using Abp.Web.Security.AntiForgery;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Relatorios;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos.Relatorios
{
    [DisableAbpAntiForgeryTokenValidation]
    public class AtendimentoRelatorioController : SWMANAGERControllerBase
    {
        private IRelatorioAtendimentoAppService _relatorioAtendimentoAppService;
        private ISessionAppService _sessionAppService;
        private readonly IUserAppService _userAppService;
        public AtendimentoRelatorioController(
            IRelatorioAtendimentoAppService relatorioAtendimentoAppService,
            ISessionAppService sessionAppService,
            IUserAppService userAppService
            )
        {
            _relatorioAtendimentoAppService = relatorioAtendimentoAppService;
            _sessionAppService = sessionAppService;
            _userAppService = userAppService;
        }

        /// <summary>
        /// Entrada para filtro de visualização do Report de produtos
        /// </summary>
        /// <returns></returns>
        //GET: Mpa/Relatorios/SaldoProduto
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> Index()
        {
            FiltroModel result = await CarregarIndex();
            //result.EhMovimentacao = false;
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/Index.csHtml", result);
        }

        /// <summary>
        /// PartialView que renderiza o relatório com os filtros selecionados no formulário
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Atendimento_Relatorio_RelatorioIntenado)]
        public async Task<ActionResult> Visualizar(FiltroModel filtro)
        {
            var relatorioInternacao = await _relatorioAtendimentoAppService.ListarRelatorio(filtro.Empresa);

            var loginInformations = await _sessionAppService.GetCurrentLoginInformations();
            //foreach (var item in relatorioInternacao.Items)
            //{
            //    //Math.Truncate(decimalNumber)
            //    string teste = Convert.ToString(Math.Round(DateTime.Now.Subtract(item.DataRegistro).TotalDays, 0));
            //   //decimal teste2 = Math.Truncate(Convert.ToDecimal(teste));
            //}
            FiltroModel _filtro = new FiltroModel();
            if (relatorioInternacao.Items.Count != 0)
            {
                _filtro.Titulo = string.Concat("Mapa Diário - ", Convert.ToString(DateTime.Now));
                //_filtro.NomeHospital = "Lipp";
                _filtro.NomeHospital = relatorioInternacao.Items[0].Empresa.NomeFantasia.ToString();
                _filtro.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                _filtro.DataHora = Convert.ToString(DateTime.Now);


                _filtro.Lista = relatorioInternacao.Items.Select(m => new TesteObjeto
                {
                    CodAtendimento = m.Codigo,
                    CodPaciente = m.Paciente == null ? string.Empty : Convert.ToString(m.Paciente.CodigoPaciente),
                    Convenio = m.Convenio == null ? string.Empty : m.Convenio.NomeFantasia,
                    DataInternacao = m.DataRegistro.ToString(),
                    Empresa = m.Empresa == null ? string.Empty : m.Empresa.NomeFantasia,
                    Leito = m.Leito == null ? string.Empty : m.Leito.Descricao,
                    Medico = m.Medico == null ? string.Empty : m.Medico.NomeCompleto,
                    Origem = m.Origem == null ? string.Empty : m.Origem.Descricao,
                    Paciente = m.Paciente == null ? string.Empty : m.Paciente.NomeCompleto,
                    UnidOrganizacional = m.UnidadeOrganizacional == null ? string.Empty : m.UnidadeOrganizacional.Descricao,
                    Idade = m.Paciente == null ? string.Empty : Convert.ToString(DateTime.Now.Year - m.Paciente.Nascimento.Year),
                    DiasInternado = m.DataRegistro == null ? string.Empty : Convert.ToString(Math.Round(DateTime.Now.Subtract(m.DataRegistro).TotalDays, 0))
                }
                ).ToList();
            }
           
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Relatorios/RelatorioInternado.aspx", _filtro);
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

        private async Task<FiltroModel> CarregarIndex()
        {
            var loginInformations = await _sessionAppService.GetCurrentLoginInformations();

            var userId = AbpSession.UserId;
            //var userEmpresas = _userAppService.GetUserEmpresas(userId.Value);
           // var userEmpresas = _relatorioAtendimentoAppService.ListarEmpresaUsuario(userId.Value);

            FiltroModel result = new FiltroModel();

            result.Empresas = ( _relatorioAtendimentoAppService.ListarEmpresaUsuario(userId.Value))
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Nome
                }).ToList();

            var padrao = new SelectListItem { Text = "Selecione", Value = "0" };
            result.Empresas.Insert(0, padrao);
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