using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Tecnicos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Tecnicos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class TecnicosController : SWMANAGERControllerBase
    {
        private readonly ITecnicoAppService _TecnicoAppService;

        public TecnicosController(
            ITecnicoAppService TecnicoAppService
            )
        {
            _TecnicoAppService = TecnicoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new TecnicosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Tecnicos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_Tecnico_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTecnicoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _TecnicoAppService.Obter((long)id);
                viewModel = new CriarOuEditarTecnicoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTecnicoModalViewModel(new TecnicoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Tecnicos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _TecnicoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}