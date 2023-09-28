using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Unidades;

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class UnidadesController : Controller //SWMANAGERControllerBase
    {
        private readonly IUnidadeAppService _unidadeAppService;

        public UnidadesController(
            IUnidadeAppService unidadeAppService
            )
        {
            _unidadeAppService = unidadeAppService;
        }
        // GET: Mpa/Unidade
        public ActionResult Index()
        {
            var model = new UnidadesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Unidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Edit)]
        [HttpPost]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarUnidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _unidadeAppService.Obter((long)id); //_Unidadeservice.GetUnidades(new GetUnidadesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarUnidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarUnidadeModalViewModel(new UnidadeDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Unidades/_CriarOuEditarModal.cshtml", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Unidade_Edit)]
        [HttpPost]
        public async Task<PartialViewResult> CriarOuEditarUnidadeModal(long? id)
        {
            CriarOuEditarUnidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _unidadeAppService.Obter((long)id); //_Unidadeservice.GetUnidades(new GetUnidadesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarUnidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarUnidadeModalViewModel(new UnidadeDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Unidades/_CriarOuEditarUnidadeModal.cshtml", viewModel);
        }

        [HttpPost]
        public JsonResult ProdutoRelacaoUnidadeListarUnidadesOptions()
        {
            try
            {
                //var sexos = await _sexoAppService.Listar();
                var unidades = AsyncHelper.RunSync(() => _unidadeAppService.ListarTodos());
                var lista = unidades.Items.ToList().Select(
                    c => new { DisplayText = string.Format("{0}{1}{2}", "     ", c.Sigla, "     "), Value = c.Id });
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