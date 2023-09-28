using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.TiposControles.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class TiposControlesController : SWMANAGERControllerBase
    {
        private readonly ITipoControleAppService _tipoControleAppService;

        public TiposControlesController(
            ITipoControleAppService tipoControleAppService
            )
        {
            _tipoControleAppService = tipoControleAppService;
        }

        public ActionResult Index()
        {
            var model = new TipoControleViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposControles/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Create, AppPermissions.Pages_Tenant_Cadastros_Assistenciais_Prescricao_TipoControle_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoControleViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoControleAppService.Obter((long)id);
                viewModel = new CriarOuEditarTipoControleViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoControleViewModel(new TipoControleDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/Prescricoes/TiposControles/_CriarOuEditarModal.cshtml", viewModel);
        }

    }

}