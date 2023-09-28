using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Setores.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Setores;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class SetoresController : SWMANAGERControllerBase
    {
        private readonly ISetorAppService _SetorAppService;

        public SetoresController(
            ISetorAppService SetorAppService
            )
        {
            _SetorAppService = SetorAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new SetoresViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Setores/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Setor_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarSetorModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _SetorAppService.Obter((long)id);
                viewModel = new CriarOuEditarSetorModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarSetorModalViewModel(new SetorDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Setores/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _SetorAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}