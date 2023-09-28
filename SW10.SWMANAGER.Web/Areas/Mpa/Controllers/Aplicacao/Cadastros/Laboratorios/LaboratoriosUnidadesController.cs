using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.LaboratoriosUnidades.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.LaboratoriosUnidades;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorio
{
    public class LaboratoriosUnidadesController : SWMANAGERControllerBase
    {
        private readonly ILaboratorioUnidadeAppService _LaboratorioUnidadeAppService;

        public LaboratoriosUnidadesController(
            ILaboratorioUnidadeAppService LaboratorioUnidadeAppService
            )
        {
            _LaboratorioUnidadeAppService = LaboratorioUnidadeAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new LaboratoriosUnidadesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/LaboratorioUnidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_LaboratorioUnidade_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarLaboratorioUnidadeModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _LaboratorioUnidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarLaboratorioUnidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarLaboratorioUnidadeModalViewModel(new LaboratorioUnidadeDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/LaboratorioUnidades/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _LaboratorioUnidadeAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}