#region Usings
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Suprimentos.EstoqueKits;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Suprimentos
{
    public class EstoqueKitsController : SWMANAGERControllerBase
    {
        #region Injecao e Contrutor

        private readonly IEstoqueKitAppService _estoqueKitAppService;

        public EstoqueKitsController(
            IEstoqueKitAppService estoqueKitAppService
            )
        {
            _estoqueKitAppService = estoqueKitAppService;
        }

        #endregion

        public async Task<ActionResult> Index()
        {
            var model = new EstoqueKitsViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Suprimentos/EstoqueKits/Index.cshtml", model);
        }

        //[AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_Suprimento_Kit_Create, AppPermissions.Pages_Tenant_Cadastros_Suprimento_Kit_Edit)]
        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarEstoqueKitModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = _estoqueKitAppService.ObterPeloId((long)id);

                viewModel = new CriarOuEditarEstoqueKitModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarEstoqueKitModalViewModel(new EstoqueKitDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Suprimentos/EstoqueKits/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}