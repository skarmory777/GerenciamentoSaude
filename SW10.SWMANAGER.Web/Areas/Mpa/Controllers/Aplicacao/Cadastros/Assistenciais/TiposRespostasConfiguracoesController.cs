using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposRespostas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class TiposRespostasConfiguracoesController : SWMANAGERControllerBase
    {
        private readonly ITipoRespostaConfiguracaoAppService _tipoRespostaConfiguracaoAppService;

        private readonly ITipoRespostaConfiguracaoElementoHtmlAppService _tipoRespostaConfiguracaoElementoHtmlAppService;

        public TiposRespostasConfiguracoesController(
            ITipoRespostaConfiguracaoAppService tipoRespostaConfiguracaoAppService,
            ITipoRespostaConfiguracaoElementoHtmlAppService tipoRespostaConfiguracaoElementoHtmlAppService
            )
        {
            _tipoRespostaConfiguracaoAppService = tipoRespostaConfiguracaoAppService;
            _tipoRespostaConfiguracaoElementoHtmlAppService = tipoRespostaConfiguracaoElementoHtmlAppService;
        }
        // GET: Mpa/TipoRespostaConfiguracao
        public ActionResult Index()
        {
            var model = new TipoRespostaConfiguracaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostasConfiguracoes/Index.cshtml", model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_TipoRespostaConfiguracaoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_TipoRespostaConfiguracaoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoRespostaConfiguracaoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoRespostaConfiguracaoAppService.Obter((long)id); //_TiposRespostasConfiguracoeservice.GetTiposRespostasConfiguracoes(new GetTiposRespostasConfiguracoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoRespostaConfiguracaoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoRespostaConfiguracaoViewModel(new TipoRespostaConfiguracaoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostasConfiguracoes/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<PartialViewResult> _CriarOuEditarRelacaoModal(long tipoRespostaConfiguracaoId, long? id)
        {
            CriarOuEditarTipoRespostaConfiguracaoElementoHtmlViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoRespostaConfiguracaoElementoHtmlAppService.Obter((long)id); //_TiposRespostasConfiguracoeservice.GetTiposRespostasConfiguracoes(new GetTiposRespostasConfiguracoesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoRespostaConfiguracaoElementoHtmlViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoRespostaConfiguracaoElementoHtmlViewModel(new TipoRespostaConfiguracaoElementoHtmlDto());
            }
            viewModel.TipoRespostaConfiguracaoId = tipoRespostaConfiguracaoId;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposRespostasConfiguracoes/_CriarOuEditarRelacaoModal.cshtml", viewModel);
        }


    }
}