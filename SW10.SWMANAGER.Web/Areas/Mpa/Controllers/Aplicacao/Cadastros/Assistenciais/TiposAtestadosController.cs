using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Atestados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Assistenciais.TiposAtestados;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Assistenciais
{
    public class TiposAtestadosController : SWMANAGERControllerBase
    {
        private readonly ITipoAtestadoAppService _tipoAtestadoAppService;

        public TiposAtestadosController(
            ITipoAtestadoAppService tipoAtestadoAppService
            )
        {
            _tipoAtestadoAppService = tipoAtestadoAppService;
        }
        // GET: Mpa/Atestado
        public ActionResult Index()
        {
            var model = new TipoAtestadoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/TiposAtestados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_AtestadoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoAtestadoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoAtestadoAppService.Obter((long)id);
                viewModel = new CriarOuEditarTipoAtestadoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoAtestadoViewModel(new TipoAtestadoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Assistenciais/TiposAtestados/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}