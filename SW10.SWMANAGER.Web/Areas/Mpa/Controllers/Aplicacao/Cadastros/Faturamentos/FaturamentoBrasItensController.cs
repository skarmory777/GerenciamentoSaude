using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Faturamentos.BrasItens;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoBrasItensController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IFaturamentoBrasItemAppService _brasItemAppService;


        public FaturamentoBrasItensController(
            IFaturamentoBrasItemAppService brasItemAppService
            )
        {
            _brasItemAppService = brasItemAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new FaturamentoBrasItensViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasItens/Index.cshtml", model);
        }

        //    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasItem_Create, AppPermissions.Pages_Tenant_Cadastros_Faturamento_BrasItem_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFaturamentoBrasItemModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _brasItemAppService.Obter((long)id);
                viewModel = new CriarOuEditarFaturamentoBrasItemModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFaturamentoBrasItemModalViewModel(new FaturamentoBrasItemDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/BrasItens/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}