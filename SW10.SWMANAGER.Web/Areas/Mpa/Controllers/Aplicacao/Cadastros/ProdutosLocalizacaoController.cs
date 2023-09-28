using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLocalizacao.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosLocalizacao;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosLocalizacaoController : Controller // SWMANAGERControllerBase
    {
        private readonly IProdutoLocalizacaoAppService _produtoLocalizacaoAppService;

        public ProdutosLocalizacaoController(
            IProdutoLocalizacaoAppService produtoLocalizacaoAppService
            )
        {
            _produtoLocalizacaoAppService = produtoLocalizacaoAppService;
        }
        // GET: Mpa/ProdutoLocalizacao
        public ActionResult Index()
        {
            var model = new ProdutosLocalizacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosLocalizacao/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_LocalizacaoProduto_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoLocalizacaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _produtoLocalizacaoAppService.Obter((long)id); //_ProdutosLocalizacaoervice.GetProdutosLocalizacao(new GetProdutosLocalizacaoInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarProdutoLocalizacaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoLocalizacaoModalViewModel(new ProdutoLocalizacaoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosLocalizacao/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetLocalizacaoOptions(long id)
        {
            try
            {
                List<ProdutoLocalizacaoDto> listObj = new List<ProdutoLocalizacaoDto>();
                IEnumerable<object> lista;
                if (id != 0)
                {
                    var obj = AsyncHelper.RunSync(() => _produtoLocalizacaoAppService.Obter(id));
                    listObj.Add(obj);
                    lista = listObj.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                else
                {
                    var produtosLocalizacao = AsyncHelper.RunSync(() => _produtoLocalizacaoAppService.ListarTodos());
                    lista = produtosLocalizacao.Items.ToList().Select(
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