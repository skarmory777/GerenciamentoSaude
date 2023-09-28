using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Paises;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class PaisesController : SWMANAGERControllerBase
    {
        private readonly IPaisAppService _paisAppService;

        public PaisesController(
            IPaisAppService paisAppService
            )
        {
            _paisAppService = paisAppService;
        }

        public ActionResult Index()
        {
            var model = new PaisesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Paises/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Pais_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarPaisModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _paisAppService.Obter((long)id);
                viewModel = new CriarOuEditarPaisModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarPaisModalViewModel(new CriarOuEditarPais());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Paises/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _paisAppService.ListarAutoComplete(term);
            //var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            //return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}