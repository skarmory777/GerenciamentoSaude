using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposUnidade.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposUnidade;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposUnidadeController : Controller //SWMANAGERControllerBase
    {
        private readonly ITipoUnidadeAppService _tipoUnidadeAppService;

        public TiposUnidadeController(
            ITipoUnidadeAppService tipoUnidadeAppService
            )
        {
            _tipoUnidadeAppService = tipoUnidadeAppService;
        }
        // GET: Mpa/TipoUnidade
        public ActionResult Index()
        {
            var model = new TiposUnidadeViewModel();
            return Content("<h1>teste</h1>");//View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposUnidade/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_TipoUnidade_Edit)]
        [HttpPost]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoUnidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoUnidadeAppService.Obter((long)id); //_TiposUnidadeervice.GetTiposUnidade(new GetTiposUnidadeInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoUnidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoUnidadeModalViewModel(new CriarOuEditarTipoUnidade());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposUnidade/_CriarOuEditarModal.cshtml", viewModel);
        }

        // [HttpPost]
        public JsonResult GetTiposUnidadeOptions()
        {
            try
            {
                //var sexos = await _sexoAppService.Listar();
                var tiposUnidade = AsyncHelper.RunSync(() => _tipoUnidadeAppService.ListarTodos());
                var lista = tiposUnidade.Items.ToList().Select(
                    c => new { DisplayText = string.Format("{0}{1}{2}", "     ", c.Descricao, "     "), Value = c.Id });
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