using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Equipamentos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Equipamentos;
using SW10.SWMANAGER.Web.Controllers;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class EquipamentosController : SWMANAGERControllerBase
    {
        private readonly IEquipamentoAppService _EquipamentoAppService;

        public EquipamentosController(
            IEquipamentoAppService EquipamentoAppService
            )
        {
            _EquipamentoAppService = EquipamentoAppService;
        }

        public async Task<ActionResult> Index()
        {
            var model = new EquipamentosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Equipamentos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarEquipamentoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _EquipamentoAppService.Obter((long)id);
                viewModel = new CriarOuEditarEquipamentoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarEquipamentoModalViewModel(new EquipamentoDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Equipamentos/_CriarOuEditarModal.cshtml", viewModel);
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _EquipamentoAppService.ListarAutoComplete(term);
            var result = query.Items.Select(m => new { m.Id, m.Nome }).ToList();
            return Json(result.ToArray(), JsonRequestBehavior.AllowGet);
            //  return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}