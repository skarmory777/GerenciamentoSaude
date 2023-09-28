using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosTransferenciaLeito.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosTransferenciaLeito;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class MotivosTransferenciaLeitoController : SWMANAGERControllerBase
    {
        private readonly IMotivoTransferenciaLeitoAppService _MotivoTransferenciaLeitoAppService;

        public MotivosTransferenciaLeitoController(
            IMotivoTransferenciaLeitoAppService MotivoTransferenciaLeitoAppService
            )
        {
            _MotivoTransferenciaLeitoAppService = MotivoTransferenciaLeitoAppService;
        }
        // GET: Mpa/MotivoTransferenciaLeito
        public ActionResult Index()
        {
            var model = new MotivosTransferenciaLeitoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosTransferenciaLeito/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosTransferenciaLeito_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarMotivoTransferenciaLeitoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _MotivoTransferenciaLeitoAppService.Obter((long)id); //_MotivosTransferenciaLeitoervice.GetMotivosTransferenciaLeito(new GetMotivosTransferenciaLeitoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarMotivoTransferenciaLeitoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarMotivoTransferenciaLeitoModalViewModel(new CriarOuEditarMotivoTransferenciaLeito());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosTransferenciaLeito/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}