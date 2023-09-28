using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposClasse.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.ProdutosClasse;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ProdutosClasseController : Controller // SWMANAGERControllerBase
    {

        private readonly IGrupoClasseAppService _produtoClasseAppService;

        public ProdutosClasseController(IGrupoClasseAppService produtoClasseAppService)
        {
            _produtoClasseAppService = produtoClasseAppService;
        }

        // GET: Mpa/Classe
        public ActionResult Index()
        {
            var model = new ProdutosClasseViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosClasse/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosSuprimentos_Classe_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarProdutoClasseModalViewModel viewModel;

            if (id.HasValue)
            {
                var output = await _produtoClasseAppService.Obter((long)id); //_Classeervice.GetClasse(new GetClasseInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                viewModel = new CriarOuEditarProdutoClasseModalViewModel(output);//.MapTo<CriarOuEditarGrupoClasse>());
            }
            else
            {
                viewModel = new CriarOuEditarProdutoClasseModalViewModel(new CriarOuEditarGrupoClasse());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/ProdutosClasse/_CriarOuEditarModal.cshtml", viewModel);
        }

        //public async Task<JsonResult> AutoComplete(string term, long id)
        //{
        //    var query = await _produtoClasseAppService.ListarAutoComplete(term, id);
        //    return Json(query.Items, JsonRequestBehavior.AllowGet);
        //}

    }
}