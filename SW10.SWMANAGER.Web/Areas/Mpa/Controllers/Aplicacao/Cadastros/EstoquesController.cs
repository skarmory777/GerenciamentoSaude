using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Estoques;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosEstoqueController : Controller // SWMANAGERControllerBase
    {
        private readonly IEstoqueAppService _estoqueAppService;
        private readonly IEstoqueAppService _tipoProdutoEstoqueAppService;

        public ProdutosEstoqueController(IEstoqueAppService produtoEstoqueAppService, IEstoqueAppService tipoProdutoEstoqueAppService)
        {
            _estoqueAppService = produtoEstoqueAppService;
            _tipoProdutoEstoqueAppService = tipoProdutoEstoqueAppService;
        }

        // GET: Mpa/ProdutoEstoque
        public ActionResult Index()
        {
            var model = new ProdutosEstoqueViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Estoques/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Estoque_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarEstoqueModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _estoqueAppService.Obter((long)id);
                viewModel = new CriarOuEditarEstoqueModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarEstoqueModalViewModel(new EstoqueDto());
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Estoques/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult getEstoquesOptions(long id)
        {
            try
            {
                List<EstoqueDto> listObj = new List<EstoqueDto>();
                IEnumerable<object> lista;
                if (id != 0)
                {
                    var obj = AsyncHelper.RunSync(() => _estoqueAppService.Obter(id));
                    listObj.Add(obj);
                    lista = listObj.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                else
                {
                    var produtosEstoque = AsyncHelper.RunSync(() => _estoqueAppService.ListarTodos());
                    lista = produtosEstoque.Items.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public async Task<JsonResult> AutoComplete(string term)
        {
            var query = await _estoqueAppService.ListarAutoComplete(term);
            return Json(query.Items.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}