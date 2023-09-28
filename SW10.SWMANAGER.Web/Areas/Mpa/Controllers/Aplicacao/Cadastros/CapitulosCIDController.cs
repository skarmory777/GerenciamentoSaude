using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.CapitulosCID;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class CapitulosCIDController : SWMANAGERControllerBase
    {
        private readonly ICapituloCIDAppService _CapituloCIDAppService;

        public CapitulosCIDController(
            ICapituloCIDAppService CapituloCIDAppService
            )
        {
            _CapituloCIDAppService = CapituloCIDAppService;
        }
        // GET: Mpa/CapituloCID
        public ActionResult Index()
        {
            var model = new CapitulosCIDViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/CapitulosCID/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_CapituloCID_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarCapituloCIDModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _CapituloCIDAppService.Obter((long)id); //_CapitulosCIDervice.GetCapitulosCID(new GetCapitulosCIDInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarCapituloCIDModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarCapituloCIDModalViewModel(new CapituloCIDDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/CapitulosCID/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}