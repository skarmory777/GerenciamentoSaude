using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Materiais.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Materiais;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class MateriaisController : SWMANAGERControllerBase
    {
        private readonly IMaterialAppService _MaterialAppService;

        public MateriaisController(
            IMaterialAppService MaterialAppService
            )
        {
            _MaterialAppService = MaterialAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = await Task.Run(() => new MateriaisViewModel());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Materiais/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Material_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarMaterialModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _MaterialAppService.Obter((long)id);
                viewModel = new CriarOuEditarMaterialModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarMaterialModalViewModel(new MaterialDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Materiais/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _MaterialAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}