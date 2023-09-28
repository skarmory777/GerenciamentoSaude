using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosTiposUnidade.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosTiposUnidade;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosTiposUnidadeController : Controller // SWMANAGERControllerBase
    {
        private readonly IProdutoTipoUnidadeAppService _produtoTipoUnidadeAppService;
        private readonly IProdutoTipoUnidadeAppService _tipoProdutoTipoUnidadeAppService;

        public ProdutosTiposUnidadeController(IProdutoTipoUnidadeAppService produtoTipoUnidadeAppService, IProdutoTipoUnidadeAppService tipoProdutoTipoUnidadeAppService)
        {
            _produtoTipoUnidadeAppService = produtoTipoUnidadeAppService;
            _tipoProdutoTipoUnidadeAppService = tipoProdutoTipoUnidadeAppService;
        }

        // GET: Mpa/ProdutoTipoUnidade
        public ActionResult Index()
        {
            var model = new ProdutosTiposUnidadeViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosTiposUnidade/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoTipoUnidadeModalViewModel viewModel;

            //var produtosTiposUnidade = await _tipoProdutoTipoUnidadeAppService.Listar();

            if (id.HasValue)
            {
                var output = await _produtoTipoUnidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoTipoUnidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoTipoUnidadeModalViewModel(new ProdutoTipoUnidadeDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosTiposUnidade/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}