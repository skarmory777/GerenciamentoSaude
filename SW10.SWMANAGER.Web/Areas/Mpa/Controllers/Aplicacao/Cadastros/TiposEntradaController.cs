using Abp.Threading;
using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposEntrada.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposEntrada;

using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposEntradaController : Controller //SWMANAGERControllerBase
    {
        private readonly ITipoEntradaAppService _tipoEntradaAppService;

        public TiposEntradaController(
            ITipoEntradaAppService tipoEntradaAppService
            )
        {
            _tipoEntradaAppService = tipoEntradaAppService;
        }
        // GET: Mpa/TipoUnidade
        public ActionResult Index()
        {
            var model = new TipoEntradaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposEntrada/Index.cshtml", model);


        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_TipoEntrada_Edit)]
        [HttpPost]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoEntradaModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoEntradaAppService.Obter((long)id); //_TiposUnidadeervice.GetTiposUnidade(new GetTiposUnidadeInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoEntradaModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoEntradaModalViewModel(new CriarOuEditarTipoEntrada());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposEntrada/_CriarOuEditarModal.cshtml", viewModel);
        }

        // [HttpPost]
        public JsonResult GetTiposEntradaOptions()
        {
            try
            {
                //var sexos = await _sexoAppService.Listar();
                var tiposEntrada = AsyncHelper.RunSync(() => _tipoEntradaAppService.ListarTodos());
                var lista = tiposEntrada.Items.ToList().Select(
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