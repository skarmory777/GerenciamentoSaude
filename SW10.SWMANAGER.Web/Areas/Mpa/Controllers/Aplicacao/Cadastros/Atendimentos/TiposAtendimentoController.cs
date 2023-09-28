using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposAtendimento.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.TiposAtendimento;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Atendimentos
{
    public class TiposAtendimentoController : SWMANAGERControllerBase
    {
        private readonly ITipoAtendimentoAppService _tipoAtendimentoAppService;

        public TiposAtendimentoController(
            ITipoAtendimentoAppService tipoAtendimentoAppService
            )
        {
            _tipoAtendimentoAppService = tipoAtendimentoAppService;
        }
        // GET: Mpa/TipoAtendimento
        public ActionResult Index()
        {
            var model = new TiposAtendimentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/TiposAtendimento/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Create, AppPermissions.Pages_Tenant_Cadastros_Atendimento_TiposAtendimento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoAtendimentoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoAtendimentoAppService.Obter((long)id); //_TiposAtendimentoervice.GetTiposAtendimento(new GetTiposAtendimentoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoAtendimentoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoAtendimentoModalViewModel(new TipoAtendimentoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/TiposAtendimento/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}