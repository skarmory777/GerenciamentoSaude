using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Metodos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Metodos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class MetodosController : SWMANAGERControllerBase
    {
        private readonly IMetodoAppService _MetodoAppService;

        public MetodosController(
            IMetodoAppService MetodoAppService
            )
        {
            _MetodoAppService = MetodoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new MetodosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Metodos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Metodo_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarMetodoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _MetodoAppService.Obter((long)id);
                viewModel = new CriarOuEditarMetodoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarMetodoModalViewModel(new MetodoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Metodos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _MetodoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}