using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ItensResultados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.ItensResultados;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class ItensResultadosController : SWMANAGERControllerBase
    {
        private readonly IItemResultadoAppService _ItemResultadoAppService;

        public ItensResultadosController(
            IItemResultadoAppService ItemResultadoAppService
            )
        {
            _ItemResultadoAppService = ItemResultadoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new ItensResultadosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ItensResultados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_ItemResultado_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarItemResultadoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _ItemResultadoAppService.Obter((long)id);
                viewModel = new CriarOuEditarItemResultadoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarItemResultadoModalViewModel(new ItemResultadoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/ItensResultados/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _ItemResultadoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}