using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Intervalos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class IntervalosController : SWMANAGERControllerBase
    {
        private readonly IIntervaloAppService _intervaloAppService;

        public IntervalosController(
            IIntervaloAppService intervaloAppService
            )
        {
            _intervaloAppService = intervaloAppService;
        }


        public ActionResult Index()
        {
            var model = new IntervalosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Intervalos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Intervalo_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {

            CriarOuEditarIntervaloModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _intervaloAppService.Obter((long)id);
                viewModel = new CriarOuEditarIntervaloModalViewModel(output);
            }
            else
            {

                viewModel = new CriarOuEditarIntervaloModalViewModel(new CriarOuEditarIntervalo());

            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Intervalos/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}