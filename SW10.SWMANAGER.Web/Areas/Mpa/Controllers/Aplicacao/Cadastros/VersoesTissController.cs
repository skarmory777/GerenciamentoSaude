using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VersoesTiss.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.VersoesTiss;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class VersoesTissController : SWMANAGERControllerBase
    {
        private readonly IVersaoTissAppService _versaoTissAppService;


        public VersoesTissController(
            IVersaoTissAppService versaoTissAppService
            )
        {
            _versaoTissAppService = versaoTissAppService;
        }


        public ActionResult Index()
        {
            var model = new VersoesTissViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/VersoesTiss/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_VersoesTiss_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {

            CriarOuEditarVersaoTissModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _versaoTissAppService.Obter((long)id);
                viewModel = new CriarOuEditarVersaoTissModalViewModel(output);
            }
            else
            {

                viewModel = new CriarOuEditarVersaoTissModalViewModel(new CriarOuEditarVersaoTiss());

            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/VersoesTiss/_CriarOuEditarModal.cshtml", viewModel);
        }

        //public async Task<JsonResult> VersoesTissPorMedico(long id)
        //{
        //	var versoesTiss = await _versaoTissAppService.Listar(id);
        //	return Json(versoesTiss,"application/json;charset=UTF-8",JsonRequestBehavior.AllowGet);
        //}

    }
}
