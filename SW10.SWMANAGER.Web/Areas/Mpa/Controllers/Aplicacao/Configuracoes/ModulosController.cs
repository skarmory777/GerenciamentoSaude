using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Modulos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.Modulos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Configuracoes
{
    public class ModulosController : SWMANAGERControllerBase
    {
        private readonly IModuloAppService _moduloAppService;

        public ModulosController(
            IModuloAppService moduloAppService
            )
        {
            _moduloAppService = moduloAppService;
        }

        public ActionResult Index()
        {
            var model = new ModulosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Modulos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Configuracoes_Modulo_Create, AppPermissions.Pages_Tenant_Configuracoes_Modulo_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarModuloModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _moduloAppService.Obter(id.Value);
                viewModel = new CriarOuEditarModuloModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarModuloModalViewModel(new ModuloDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Modulos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}