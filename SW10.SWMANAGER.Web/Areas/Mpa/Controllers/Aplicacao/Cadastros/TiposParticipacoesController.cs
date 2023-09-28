using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposParticipacoes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposParticipacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposParticipacoesController : SWMANAGERControllerBase
    {
        private readonly ITipoParticipacaoAppService _TipoParticipacaoAppService;

        public TiposParticipacoesController(
            ITipoParticipacaoAppService TipoParticipacaoAppService
            )
        {
            _TipoParticipacaoAppService = TipoParticipacaoAppService;
        }
        // GET: Mpa/TipoParticipacao
        public ActionResult Index()
        {
            var model = new TiposParticipacoesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposParticipacoes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoParticipacao_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoParticipacaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _TipoParticipacaoAppService.Obter((long)id); //_TiposParticipacoeservice.GetTiposParticipacoes(new GetTiposParticipacoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoParticipacaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoParticipacaoModalViewModel(new TipoParticipacaoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposParticipacoes/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}