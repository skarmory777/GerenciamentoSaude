using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposSanguineos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposSanguineos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposSanguineosController : SWMANAGERControllerBase
    {
        private readonly ITipoSanguineoAppService _tipoSanguineoAppService;

        public TiposSanguineosController(
            ITipoSanguineoAppService tipoSanguineoAppService
            )
        {
            _tipoSanguineoAppService = tipoSanguineoAppService;
        }
        // GET: Mpa/TipoSanguineo
        public ActionResult Index()
        {
            var model = new TiposSanguineosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposSanguineos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoSanguineo_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoSanguineoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoSanguineoAppService.Obter((long)id); //_TiposSanguineoservice.GetTiposSanguineos(new GetTiposSanguineosInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoSanguineoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoSanguineoModalViewModel(new TipoSanguineoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposSanguineos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}