using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class CentrosCustosController : SWMANAGERControllerBase
    {
        #region Cabecalho
        private readonly ICentroCustoAppService _centroCustoAppService;
        private readonly IGrupoCentroCustoAppService _grupoCentroCustoAppService;
        private readonly IUnidadeOrganizacionalAppService _unidadeOrganizacionalAppService;

        public CentrosCustosController(
            ICentroCustoAppService centroCustoAppService,
            IGrupoCentroCustoAppService grupoCentroCustoAppService,
            IUnidadeOrganizacionalAppService unidadeOrganizacionalAppService
            )
        {
            _centroCustoAppService = centroCustoAppService;
            _grupoCentroCustoAppService = grupoCentroCustoAppService;
            _unidadeOrganizacionalAppService = unidadeOrganizacionalAppService;
        }
        #endregion cabecalho.

        // GET: Mpa/CentroCusto
        public ActionResult Index()
        {
            var model = new CentrosCustosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/CentrosCustos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CentrosCustos_Create)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarCentroCustoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _centroCustoAppService.Obter((long)id);
                viewModel = new CriarOuEditarCentroCustoModalViewModel(output);
                viewModel.UnidadeOrganizacional = await _unidadeOrganizacionalAppService.Obter(output.UnidadeOrganizacionalId);
                viewModel.GrupoCentroCusto = await _grupoCentroCustoAppService.Obter(output.GrupoCentroCustoId);

            }
            else
            {
                viewModel = new CriarOuEditarCentroCustoModalViewModel(new CriarOuEditarCentroCusto());
                viewModel.IsAtivo = true;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/CentrosCustos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _centroCustoAppService.ListarAutoComplete(term);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}