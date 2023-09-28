using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Indicacoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Indicacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class IndicacoesController : SWMANAGERControllerBase
    {
        private readonly IIndicacaoAppService _indicacaoAppService;

        public IndicacoesController(
            IIndicacaoAppService indicacaoAppService
            )
        {
            _indicacaoAppService = indicacaoAppService;
        }
        // GET: Mpa/Indicacao
        public ActionResult Index()
        {
            var model = new IndicacoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Indicacoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Indicacao_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarIndicacaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _indicacaoAppService.Obter((long)id); //_Indicacoeservice.GetIndicacoes(new GetIndicacoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarIndicacaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarIndicacaoModalViewModel(new CriarOuEditarIndicacao());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Indicacoes/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}