using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Parentescos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Parentescos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class ParentescosController : SWMANAGERControllerBase
    {
        private readonly IParentescoAppService _ParentescoAppService;

        public ParentescosController(
            IParentescoAppService ParentescoAppService
            )
        {
            _ParentescoAppService = ParentescoAppService;
        }
        // GET: Mpa/Parentesco
        public ActionResult Index()
        {
            var model = new ParentescosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Parentescos/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Parentesco_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarParentescoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _ParentescoAppService.Obter((long)id); //_Parentescoservice.GetParentescos(new GetParentescosInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarParentescoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarParentescoModalViewModel(new CriarOuEditarParentesco());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Parentescos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}