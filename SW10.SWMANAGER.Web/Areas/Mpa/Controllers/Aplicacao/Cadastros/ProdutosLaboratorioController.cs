using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosLaboratorio;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosLaboratorioController : Controller // SWMANAGERControllerBase
    {
        private readonly IProdutoLaboratorioAppService _produtoLaboratorioAppService;
        private readonly IFaturamentoBrasLaboratorioAppService _faturamentoBrasLaboratorioAppService;

        public ProdutosLaboratorioController(
               IProdutoLaboratorioAppService laboratorioAppService,
               IProdutoLaboratorioAppService tipoProdutoLaboratorioAppService,
               IFaturamentoBrasLaboratorioAppService faturamentoBrasLaboratorioAppService
               )
        {
            _produtoLaboratorioAppService = laboratorioAppService;
            _faturamentoBrasLaboratorioAppService = faturamentoBrasLaboratorioAppService;

        }

        // GET: Mpa/ProdutoLaboratorio
        public ActionResult Index()
        {
            var model = new ProdutosLaboratorioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosLaboratorio/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Laboratorio_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoLaboratorioModalViewModel viewModel;

            //var produtosLaboratorio = await _tipoLaboratorioAppService.Listar();

            if (id.HasValue)
            {
                var output = await _produtoLaboratorioAppService.Obter((long)id);
                viewModel = new CriarOuEditarProdutoLaboratorioModalViewModel(output);
                viewModel.BrasLaboratorio = await _faturamentoBrasLaboratorioAppService.Obter(output.BrasLaboratorioId);
            }
            else
            {
                viewModel = new CriarOuEditarProdutoLaboratorioModalViewModel(new ProdutoLaboratorioDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosLaboratorio/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AcceptVerbs("GET", "POST", "PUT")]
        public JsonResult GetLaboratoriosOptions(long id)
        {

            try
            {
                List<ProdutoLaboratorioDto> listObj = new List<ProdutoLaboratorioDto>();
                IEnumerable<object> lista;
                if (id != 0)
                {
                    var obj = AsyncHelper.RunSync(() => _produtoLaboratorioAppService.Obter(id));
                    listObj.Add(obj);
                    lista = listObj.ToList().Select(
                        c => new { DisplayText = string.Format("{0}", c.Descricao), Value = c.Id });
                }
                else
                {
                    var produtosLaboratorio = AsyncHelper.RunSync(() => _produtoLaboratorioAppService.ListarTodos());
                    lista = produtosLaboratorio.Items.ToList().Select(
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