using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosAcoesTerapeutica.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosAcoesTerapeutica;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosAcoesTerapeuticaController : Controller //SWMANAGERControllerBase
    {

        private readonly IProdutoAcaoTerapeuticaAppService _produtoAcaoTerapeuticaAppService;
        private readonly IProdutoAcaoTerapeuticaAppService _tipoProdutoAcaoTerapeuticaAppService;

        public ProdutosAcoesTerapeuticaController(
            IProdutoAcaoTerapeuticaAppService produtoAcaoTerapeuticaAppService,
            IProdutoAcaoTerapeuticaAppService tipoProdutoAcaoTerapeuticaAppService
            )
        {
            _produtoAcaoTerapeuticaAppService = produtoAcaoTerapeuticaAppService;
            _tipoProdutoAcaoTerapeuticaAppService = tipoProdutoAcaoTerapeuticaAppService;
        }

        // GET: Mpa/ProdutoAcaoTerapeutica
        public ActionResult Index()
        {
            var model = new ProdutosAcoesTerapeuticaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosAcoesTerapeutica/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_AcaoTerapeutica_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoAcaoTerapeuticaModalViewModel viewModel;

            //var produtosAcoesTerapeutica = await _tipoProdutoAcaoTerapeuticaAppService.Listar();

            if (id.HasValue)
            {
                var output = await _produtoAcaoTerapeuticaAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoAcaoTerapeuticaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoAcaoTerapeuticaModalViewModel(new ProdutoAcaoTerapeuticaDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosAcoesTerapeutica/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetAcaoTerapeuticasOptions(long id)
        {
            try
            {
                List<ProdutoAcaoTerapeuticaDto> listObj = new List<ProdutoAcaoTerapeuticaDto>();
                IEnumerable<object> lista;
                if (id != 0)
                {
                    var obj = AsyncHelper.RunSync(() => _produtoAcaoTerapeuticaAppService.Obter(id));
                    listObj.Add(obj);
                    lista = listObj.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                else
                {
                    var produtosAcaoTerapeutica = AsyncHelper.RunSync(() => _produtoAcaoTerapeuticaAppService.ListarTodos());
                    lista = produtosAcaoTerapeutica.Items.ToList().Select(
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