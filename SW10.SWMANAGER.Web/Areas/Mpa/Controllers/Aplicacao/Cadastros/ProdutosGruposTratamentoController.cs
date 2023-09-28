using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosGruposTratamento.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosGruposTratamento;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosGruposTratamentoController : Controller // SWMANAGERControllerBase
    {
        private readonly IProdutoGrupoTratamentoAppService _produtoGrupoTratamentoAppService;
        private readonly IProdutoGrupoTratamentoAppService _tipoProdutoGrupoTratamentoAppService;

        public ProdutosGruposTratamentoController(IProdutoGrupoTratamentoAppService produtoGrupoTratamentoAppService, IProdutoGrupoTratamentoAppService tipoProdutoGrupoTratamentoAppService)
        {
            _produtoGrupoTratamentoAppService = produtoGrupoTratamentoAppService;
            _tipoProdutoGrupoTratamentoAppService = tipoProdutoGrupoTratamentoAppService;
        }

        // GET: Mpa/ProdutoGrupoTratamento
        public ActionResult Index()
        {
            var model = new ProdutosGruposTratamentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosGruposTratamento/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_GrupoTratamento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoGrupoTratamentoModalViewModel viewModel;

            //var produtosGruposTratamento = await _tipoProdutoGrupoTratamentoAppService.Listar();

            if (id.HasValue)
            {
                var output = await _produtoGrupoTratamentoAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoGrupoTratamentoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoGrupoTratamentoModalViewModel(new CriarOuEditarProdutoGrupoTratamento());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosGruposTratamento/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}