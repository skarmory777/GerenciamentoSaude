using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoBrasLaboratoriosController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoBrasLaboratorioAppService _brasLaboratorioAppService;


        public FaturamentoBrasLaboratoriosController(
            IFaturamentoBrasLaboratorioAppService brasLaboratorioAppService
            )
        {
            _brasLaboratorioAppService = brasLaboratorioAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoBrasLaboratoriosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasLaboratorios/Index.cshtml", model);
        }

        //    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasLaboratorio_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFaturamentoBrasLaboratorioModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _brasLaboratorioAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoBrasLaboratorioModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoBrasLaboratorioModalViewModel(new FaturamentoBrasLaboratorioDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasLaboratorios/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}