using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasImports;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoBrasImportsController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoBrasImportAppService _brasImportAppService;


        public FaturamentoBrasImportsController(
            IFaturamentoBrasImportAppService brasImportAppService
            )
        {
            _brasImportAppService = brasImportAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoBrasImportsViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasImports/Index.cshtml", model);
        }

        //    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasImport_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasImport_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFaturamentoBrasImportModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _brasImportAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoBrasImportModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoBrasImportModalViewModel(new FaturamentoBrasImportDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasImports/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}