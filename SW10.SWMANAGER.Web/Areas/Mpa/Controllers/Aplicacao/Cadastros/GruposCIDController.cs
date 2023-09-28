using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCID.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GruposCID;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GruposCIDController : SWMANAGERControllerBase
    {
        private readonly IGrupoCIDAppService _grupoCIDAppService;

        public GruposCIDController(
            IGrupoCIDAppService grupoCIDAppService
            )
        {
            _grupoCIDAppService = grupoCIDAppService;
        }
        // GET: Mpa/GrupoCID
        public ActionResult Index()
        {
            var model = new GruposCIDViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposCID/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrupoCID_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarGrupoCIDModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _grupoCIDAppService.Obter((long)id); //_GruposCIDervice.GetGruposCID(new GetGruposCIDInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarGrupoCIDModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarGrupoCIDModalViewModel(new GrupoCIDDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GruposCID/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}