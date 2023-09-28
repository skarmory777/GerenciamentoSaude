using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs;
using SW10.SWMANAGER.ClassesAplicacao.Services.Manutencoes.BIs.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Manutencoes.BIs;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Manutencoes
{
    public class BIsController : SWMANAGERControllerBase
    {
        private readonly IBiAppService _frequenciaAppService;

        public BIsController(
            IBiAppService frequenciaAppService
            )
        {
            _frequenciaAppService = frequenciaAppService;
        }

        public ActionResult Index()
        {
            var model = new BIViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Manutencoes/BIs/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Manutencao_BI_Create, AppPermissions.Pages_Tenant_Manutencao_BI_Edit)]
        public async Task<PartialViewResult> CriarOuEditar(long? id)
        {
            CriarOuEditarBIViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _frequenciaAppService.Obter(id.Value);
                viewModel = new CriarOuEditarBIViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarBIViewModel(new BIDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Manutencoes/BIs/_CriarOuEditarModal.cshtml", viewModel);
        }


    }

}