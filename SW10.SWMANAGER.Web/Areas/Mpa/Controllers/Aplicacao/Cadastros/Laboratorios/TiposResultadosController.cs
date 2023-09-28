using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.TiposResultados;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class TiposResultadosController : SWMANAGERControllerBase
    {
        private readonly ITipoResultadoAppService _TipoResultadoAppService;

        public TiposResultadosController(
            ITipoResultadoAppService TipoResultadoAppService
            )
        {
            _TipoResultadoAppService = TipoResultadoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new TiposResultadosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/TipoResultados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_TipoResultado_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoResultadoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _TipoResultadoAppService.Obter((long)id);
                viewModel = new CriarOuEditarTipoResultadoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoResultadoModalViewModel(new TipoResultadoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/TipoResultados/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _TipoResultadoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}