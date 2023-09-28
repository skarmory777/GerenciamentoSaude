using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.ModelosAtestados;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class ModelosAtestadosController : SWMANAGERControllerBase
    {
        private readonly IModeloAtestadoAppService _modeloAtestadoAppService;

        public ModelosAtestadosController(
            IModeloAtestadoAppService modeloAtestadoAppService
            )
        {
            _modeloAtestadoAppService = modeloAtestadoAppService;
        }
        // GET: Mpa/Atestado
        public ActionResult Index()
        {
            var model = new ModeloAtestadoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/ModelosAtestados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarModeloAtestadoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _modeloAtestadoAppService.Obter((long)id);
                viewModel = new CriarOuEditarModeloAtestadoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarModeloAtestadoViewModel(new ModeloAtestadoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/ModelosAtestados/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}