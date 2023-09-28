using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Cabecalho;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.Cabecalho;
using SW10.SWMANAGER.Web.Controllers;

using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Laboratorios
{
    public class CabecalhosController : SWMANAGERControllerBase
    {
        private readonly ICabecalhoAppService _cabecalhoAppService;

        public CabecalhosController(ICabecalhoAppService cabecalhoAppService)
        {
            _cabecalhoAppService = cabecalhoAppService;
        }

        public async Task<ActionResult> Index()
        {

            var parametro = _cabecalhoAppService.Obter();

            var model = new CriarOuEditarCabecalhoExameViewModel(parametro);
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Cabecalho/_CriarOuEditarModal.cshtml", model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Create, AppPermissions.Pages_Tenant_Laboratorio_Cadastros_EquipamentoInterfaceamento_Edit)]
        //public async Task<ActionResult> CriarOuEditarModal(long? id)
        //{
        //    CriarOuEditarEquipamentoModalViewModel viewModel;

        //    if (id.HasValue)
        //    {
        //        var output = await _EquipamentoAppService.Obter((long)id);
        //        viewModel = new CriarOuEditarEquipamentoModalViewModel(output);
        //    }
        //    else
        //    {
        //        viewModel = new CriarOuEditarEquipamentoModalViewModel(new EquipamentoDto());
        //    }

        //    return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/Equipamentos/_CriarOuEditarModal.cshtml", viewModel);
        //}



    }
}