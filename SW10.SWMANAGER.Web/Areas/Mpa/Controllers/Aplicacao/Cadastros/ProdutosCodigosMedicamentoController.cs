using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosCodigosMedicamento.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosCodigosMedicamento;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosCodigosMedicamentoController : Controller // SWMANAGERControllerBase
    {
        private readonly IProdutoCodigoMedicamentoAppService _produtoCodigoMedicamentoAppService;

        public ProdutosCodigosMedicamentoController(IProdutoCodigoMedicamentoAppService produtoCodigoMedicamentoAppService)
        {
            _produtoCodigoMedicamentoAppService = produtoCodigoMedicamentoAppService;
        }

        // GET: Mpa/Classe
        public ActionResult Index()
        {
            var model = new ProdutosCodigoMedicamentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosCodigosMedicamento/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_ProdutosCodigoMedicamento_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoCodigoMedicamentoModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _produtoCodigoMedicamentoAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoCodigoMedicamentoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoCodigoMedicamentoModalViewModel(new ProdutoCodigoMedicamentoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosCodigosMedicamento/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}