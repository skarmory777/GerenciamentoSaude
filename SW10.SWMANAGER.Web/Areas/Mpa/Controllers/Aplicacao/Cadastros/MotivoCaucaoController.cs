using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.MotivosCaucao.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.MotivosCaucao;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class MotivosCaucaoController : SWMANAGERControllerBase
    {
        private readonly IMotivoCaucaoAppService _MotivoCaucaoAppService;

        public MotivosCaucaoController(
            IMotivoCaucaoAppService MotivoCaucaoAppService
            )
        {
            _MotivoCaucaoAppService = MotivoCaucaoAppService;
        }
        // GET: Mpa/MotivoCaucao
        public ActionResult Index()
        {
            var model = new MotivosCaucaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCaucao/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_MotivosCaucao_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarMotivoCaucaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _MotivoCaucaoAppService.Obter((long)id); //_MotivosCaucaoervice.GetMotivosCaucao(new GetMotivosCaucaoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarMotivoCaucaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarMotivoCaucaoModalViewModel(new CriarOuEditarMotivoCaucao());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/MotivosCaucao/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}