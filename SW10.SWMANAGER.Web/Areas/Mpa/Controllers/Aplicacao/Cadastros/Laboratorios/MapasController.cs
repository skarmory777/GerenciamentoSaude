using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Mapas;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class MapasController : SWMANAGERControllerBase
    {
        private readonly IMapaAppService _MapaAppService;

        public MapasController(
            IMapaAppService MapaAppService
            )
        {
            _MapaAppService = MapaAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new MapasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Mapas/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Mapa_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarMapaModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _MapaAppService.Obter((long)id);
                viewModel = new CriarOuEditarMapaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarMapaModalViewModel(new MapaDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Mapas/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _MapaAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}