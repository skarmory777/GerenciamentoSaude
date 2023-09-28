using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposVinculosEmpregaticios.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposVinculosEmpregaticios;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposVinculosEmpregaticiosController : SWMANAGERControllerBase
    {
        private readonly ITipoVinculoEmpregaticioAppService _TipoVinculoEmpregaticioAppService;

        public TiposVinculosEmpregaticiosController(
            ITipoVinculoEmpregaticioAppService TipoVinculoEmpregaticioAppService
            )
        {
            _TipoVinculoEmpregaticioAppService = TipoVinculoEmpregaticioAppService;
        }
        // GET: Mpa/TipoVinculoEmpregaticio
        public ActionResult Index()
        {
            var model = new TiposVinculosEmpregaticiosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposVinculosEmpregaticios/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoVinculoEmpregaticio_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoVinculoEmpregaticioModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _TipoVinculoEmpregaticioAppService.Obter((long)id); //_TiposVinculosEmpregaticioservice.GetTiposVinculosEmpregaticios(new GetTiposVinculosEmpregaticiosInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoVinculoEmpregaticioModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoVinculoEmpregaticioModalViewModel(new TipoVinculoEmpregaticioDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposVinculosEmpregaticios/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}