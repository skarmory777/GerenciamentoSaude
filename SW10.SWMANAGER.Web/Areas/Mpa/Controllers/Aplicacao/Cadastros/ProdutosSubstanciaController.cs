using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosSubstancia.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosSubstancia;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosSubstanciaController : Controller // SWMANAGERControllerBase
    {
        private readonly IProdutoSubstanciaAppService _ProdutoSubstanciaAppService;
        private readonly IProdutoSubstanciaAppService _tipoProdutoSubstanciaAppService;

        public ProdutosSubstanciaController(IProdutoSubstanciaAppService ProdutoSubstanciaAppService, IProdutoSubstanciaAppService tipoProdutoSubstanciaAppService)
        {
            _ProdutoSubstanciaAppService = ProdutoSubstanciaAppService;
            _tipoProdutoSubstanciaAppService = tipoProdutoSubstanciaAppService;
        }

        // GET: Mpa/ProdutoSubstancia
        public ActionResult Index()
        {
            var model = new ProdutosSubstanciaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosSubstancia/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Substancia_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoSubstanciaModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _ProdutoSubstanciaAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoSubstanciaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoSubstanciaModalViewModel(new CriarOuEditarProdutoSubstancia());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosSubstancia/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}