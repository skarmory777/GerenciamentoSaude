using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposLeito.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposLeito;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposLeitoController : SWMANAGERControllerBase
    {
        private readonly ITipoLeitoAppService _tipoLeitoAppService;

        public TiposLeitoController(
            ITipoLeitoAppService tipoLeitoAppService
            )
        {
            _tipoLeitoAppService = tipoLeitoAppService;
        }
        // GET: Mpa/TipoLeito
        public ActionResult Index()
        {
            var model = new TiposLeitoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposLeito/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposLeito_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoLeitoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoLeitoAppService.Obter((long)id); //_TiposLeitoervice.GetTiposLeito(new GetTiposLeitoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoLeitoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoLeitoModalViewModel(new CriarOuEditarTipoLeito());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposLeito/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}