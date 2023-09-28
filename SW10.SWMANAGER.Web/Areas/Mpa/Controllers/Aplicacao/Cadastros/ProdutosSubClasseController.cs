using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposSubClasse.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposSubClasse;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosSubClasse;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GruposSubClasseController : Controller // SWMANAGERControllerBase
    {
        private readonly IGrupoSubClasseAppService _grupoSubClasseAppService;
        private readonly IGrupoSubClasseAppService _tipoGrupoSubClasseAppService;

        public GruposSubClasseController(IGrupoSubClasseAppService GrupoSubClasseAppService, IGrupoSubClasseAppService tipoGrupoSubClasseAppService)
        {
            _grupoSubClasseAppService = GrupoSubClasseAppService;
            _tipoGrupoSubClasseAppService = tipoGrupoSubClasseAppService;
        }

        // GET: Mpa/GrupoSubClasse
        public ActionResult Index()
        {
            var model = new GruposSubClasseViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposSubClasse/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_SubClasse_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarGrupoSubClasseModalViewModel viewModel;

            //var GruposSubClasse = await _tipoGrupoSubClasseAppService.Listar();

            if (id.HasValue)
            {
                var output = await _grupoSubClasseAppService.Obter((long)id);
                viewModel = new CriarOuEditarGrupoSubClasseModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarGrupoSubClasseModalViewModel(new CriarOuEditarGrupoSubClasse());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposSubClasse/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term, long id)
        {
            var query = await _grupoSubClasseAppService.ListarAutoComplete(term, id);
            return Json(query.Items, JsonRequestBehavior.AllowGet);
        }

    }
}