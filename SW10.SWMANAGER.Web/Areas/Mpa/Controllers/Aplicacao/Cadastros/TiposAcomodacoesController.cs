using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposAcomodacao.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposAcomodacaoController : SWMANAGERControllerBase
    {
        private readonly ITipoAcomodacaoAppService _TipoAcomodacaoAppService;

        public TiposAcomodacaoController(
            ITipoAcomodacaoAppService TipoAcomodacaoAppService
            )
        {
            _TipoAcomodacaoAppService = TipoAcomodacaoAppService;
        }
        // GET: Mpa/TipoAcomodacao
        public ActionResult Index()
        {
            var model = new TiposAcomodacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposAcomodacao/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TiposAcomodacao_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoAcomodacaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _TipoAcomodacaoAppService.Obter((long)id); //_TiposAcomodacaoervice.GetTiposAcomodacao(new GetTiposAcomodacaoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                                                                              //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoAcomodacaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoAcomodacaoModalViewModel(new TipoAcomodacaoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposAcomodacao/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}