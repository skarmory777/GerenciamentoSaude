using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLogradouros;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposLogradouro;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposLogradouroController : SWMANAGERControllerBase
    {
        private readonly ITipoLogradouroAppService _tipoLogradouroAppService;

        public TiposLogradouroController(ITipoLogradouroAppService tipoLogradouroAppService)
        {
            _tipoLogradouroAppService = tipoLogradouroAppService;
        }

        // GET: Mpa/TiposLogradouros
        public ActionResult Index()
        {
            var model = new TiposLogradouroViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposLogradouro/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLogradouro_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTiposLogradouroModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoLogradouroAppService.Obter((long)id);
                viewModel = new CriarOuEditarTiposLogradouroModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTiposLogradouroModalViewModel(new ClassesAplicacao.Services.Cadastros.TiposLogradouros.Dto.CriarOuEditarTipoLogradouroDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposLogradouro/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}