using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GrausInstrucoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.GrausInstrucoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class GrausInstrucoesController : SWMANAGERControllerBase
    {
        private readonly IGrauInstrucaoAppService _grauIntrucaoAppService;

        public GrausInstrucoesController(
            IGrauInstrucaoAppService grauIntrucaoAppService
            )
        {
            _grauIntrucaoAppService = grauIntrucaoAppService;
        }
        // GET: Mpa/GrauInstrucao
        public ActionResult Index()
        {
            var model = new GrausInstrucoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/GrausInstrucoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_GrauInstrucao_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarGrauInstrucaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _grauIntrucaoAppService.Obter((long)id); //_GrausIntrucoeservice.GetGrausIntrucoes(new GetGrausIntrucoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarGrauInstrucaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarGrauInstrucaoModalViewModel(new CriarOuEditarGrauInstrucao());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/GrausInstrucoes/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}