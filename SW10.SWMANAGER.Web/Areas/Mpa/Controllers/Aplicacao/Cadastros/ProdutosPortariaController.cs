using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPortaria.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosPortaria;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosPortariaController : Controller//SWMANAGERControllerBase
    {
        private readonly IProdutoPortariaAppService _produtoPortariaAppService;
        private readonly IProdutoPortariaAppService _tipoProdutoPortariaAppService;

        public ProdutosPortariaController(IProdutoPortariaAppService produtoPortariaAppService, IProdutoPortariaAppService tipoProdutoPortariaAppService)
        {
            _produtoPortariaAppService = produtoPortariaAppService;
            _tipoProdutoPortariaAppService = tipoProdutoPortariaAppService;
        }

        // GET: Mpa/ProdutoPortaria
        public ActionResult Index()
        {
            var model = new ProdutosPortariaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosPortaria/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Portaria_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoPortariaModalViewModel viewModel;

            //var produtosPortaria = await _tipoProdutoPortariaAppService.Listar();

            if (id.HasValue)
            {
                var output = await _produtoPortariaAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoPortariaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoPortariaModalViewModel(new CriarOuEditarProdutoPortaria());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosPortaria/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetPortariasOptions(long id)
        {
            try
            {
                List<CriarOuEditarProdutoPortaria> listObj = new List<CriarOuEditarProdutoPortaria>();
                IEnumerable<object> lista;
                if (id != 0)
                {
                    var obj = AsyncHelper.RunSync(() => _produtoPortariaAppService.Obter(id));
                    listObj.Add(obj);
                    lista = listObj.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                else
                {
                    var produtosPortaria = AsyncHelper.RunSync(() => _produtoPortariaAppService.ListarTodos());
                    lista = produtosPortaria.Items.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}