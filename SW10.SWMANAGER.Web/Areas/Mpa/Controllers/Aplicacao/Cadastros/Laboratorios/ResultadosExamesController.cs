using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosLaudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.ResultadosExames;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class ResultadosExamesController : SWMANAGERControllerBase
    {
        private readonly IResultadoExameAppService _ResultadoExameAppService;
        private readonly IResultadoAppService _resultadoAppService;
        private readonly IUserAppService _userAppService;
        private readonly IUnidadeOrganizacionalAppService _unidadeOrganizacionalAppService;
        private readonly IResultadoLaudoAppService _ResultadoLaudoAppService;

        public ResultadosExamesController(
            IResultadoExameAppService ResultadoExameAppService,
            IResultadoAppService resultadoAppService,
            IUserAppService userAppService,
            IUnidadeOrganizacionalAppService unidadeOrganizacionalAppService,
            IResultadoLaudoAppService ResultadoLaudoAppService)
        {
            _ResultadoExameAppService = ResultadoExameAppService;
            _resultadoAppService = resultadoAppService;
            _userAppService = userAppService;
            _unidadeOrganizacionalAppService = unidadeOrganizacionalAppService;
            _ResultadoLaudoAppService = ResultadoLaudoAppService;
        }

        public async Task<ActionResult> Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadosExames/Index.cshtml");
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ResultadoExame_Edit)]
        public ActionResult CriarOuEditarModal(long? id, long? resultadoId)
        {
            CriarOuEditarResultadoExameModalViewModel viewModel;

            if (id.HasValue && id.Value != 0)
            {
                var output = Task.Run(() => _ResultadoExameAppService.Obter(id.Value)).Result;
                viewModel = new CriarOuEditarResultadoExameModalViewModel(output);
                if (viewModel.Resultado == null)
                {
                    if (!viewModel.ResultadoId.HasValue)
                    {
                        viewModel.ResultadoId = resultadoId;
                    }
                    var resultado = Task.Run(() => _resultadoAppService.Obter(viewModel.ResultadoId.Value)).Result;
                    viewModel.Resultado = resultado;
                }
            }
            else
            {
                viewModel = new CriarOuEditarResultadoExameModalViewModel(new ResultadoExameDto());
                if (resultadoId.HasValue && resultadoId.Value > 0)
                {
                    viewModel.ResultadoId = resultadoId;
                    var resultado = Task.Run(() => _resultadoAppService.Obter(resultadoId.Value)).Result;
                    viewModel.Resultado = resultado;
                }
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadosExames/_CriarOuEditarModal.cshtml", viewModel);
        }


        public PartialViewResult Legenda()
        {
            var model = Task.Run(() => _ResultadoExameAppService.Legenda()).Result;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ResultadosExames/_Legenda.cshtml", model.Items.ToList());
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await this._ResultadoExameAppService.ListarAutoComplete(term).ConfigureAwait(false);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> CriarOuEditarLista(string input, long coletaId)
        {
            var result = await this._ResultadoLaudoAppService.CriarOuEditarLista(input, coletaId).ConfigureAwait(false);
            //result = result;
            return this.Json(result, JsonRequestBehavior.DenyGet);

        }
    }
}