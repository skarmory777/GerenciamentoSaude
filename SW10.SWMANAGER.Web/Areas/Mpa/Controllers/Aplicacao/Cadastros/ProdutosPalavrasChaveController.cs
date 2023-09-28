using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosPalavrasChave.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosPalavrasChave;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosPalavrasChaveController : Controller ///SWMANAGERControllerBase
    {

        private readonly IProdutoPalavraChaveAppService _produtoPalavraChaveAppService;
        private readonly IProdutoPalavraChaveAppService _tipoProdutoPalavraChaveAppService;

        public ProdutosPalavrasChaveController(
            IProdutoPalavraChaveAppService produtoPalavraChaveAppService,
            IProdutoPalavraChaveAppService tipoProdutoPalavraChaveAppService
            )
        {
            _produtoPalavraChaveAppService = produtoPalavraChaveAppService;
            _tipoProdutoPalavraChaveAppService = tipoProdutoPalavraChaveAppService;
        }

        // GET: Mpa/ProdutoPalavraChave
        public ActionResult Index()
        {
            var model = new ProdutosPalavrasChaveViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosPalavrasChave/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_PalavraChave_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoPalavraChaveModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _produtoPalavraChaveAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoPalavraChaveModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoPalavraChaveModalViewModel(new ProdutoPalavraChaveDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosPalavrasChave/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult getPalavrasChavesOptions(long id)
        {
            try
            {
                List<ProdutoPalavraChaveDto> listObj = new List<ProdutoPalavraChaveDto>();
                IEnumerable<object> lista;
                if (id != 0)
                {
                    var obj = AsyncHelper.RunSync(() => _produtoPalavraChaveAppService.Obter(id));
                    listObj.Add(obj);
                    lista = listObj.ToList().Select(
                        //c => new { DisplayText = string.Format("{0}{1}{2}", "     ", c.Nome, "     "), Value = c.Id });
                        c => new { DisplayText = string.Format("{0}", c.Palavra), Value = c.Id });
                }
                else
                {
                    var produtosPalavraChave = AsyncHelper.RunSync(() => _produtoPalavraChaveAppService.ListarTodos());
                    lista = produtosPalavraChave.Items.ToList().Select(
                        //c => new { DisplayText = string.Format("{0}{1}{2}", "     ", c.Nome, "     "), Value = c.Id });
                        c => new { DisplayText = string.Format("{0}", c.Palavra), Value = c.Id });
                }
                return Json(new { Result = "OK", Options = lista }, JsonRequestBehavior.AllowGet);
                //@"[{""Value"":""1"", ""DisplayText"":""Teste1""},{""Value"":""2"", ""DisplayText"":""Teste2""}]" },JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}