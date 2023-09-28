using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCancelamento.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosCancelamento;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class MotivosCancelamentoController : SWMANAGERControllerBase
    {
        private readonly IMotivoCancelamentoAppService _MotivoCancelamentoAppService;

        public MotivosCancelamentoController(
            IMotivoCancelamentoAppService MotivoCancelamentoAppService
            )
        {
            _MotivoCancelamentoAppService = MotivoCancelamentoAppService;
        }
        // GET: Mpa/MotivoCancelamento
        public ActionResult Index()
        {
            var model = new MotivosCancelamentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCancelamento/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCancelamento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarMotivoCancelamentoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _MotivoCancelamentoAppService.Obter((long)id); //_MotivosCancelamentoervice.GetMotivosCancelamento(new GetMotivosCancelamentoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarMotivoCancelamentoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarMotivoCancelamentoModalViewModel(new CriarOuEditarMotivoCancelamento());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCancelamento/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}