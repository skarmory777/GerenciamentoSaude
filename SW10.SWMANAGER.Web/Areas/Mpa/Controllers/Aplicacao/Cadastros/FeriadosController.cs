using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Feriados;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FeriadosController : SWMANAGERControllerBase
    {
        private readonly IFeriadoAppService _feriadoAppService;

        public FeriadosController(
            IFeriadoAppService feriadoAppService
            )
        {
            _feriadoAppService = feriadoAppService;
        }
        // GET: Mpa/Feriado
        public ActionResult Index()
        {
            var model = new FeriadosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Feriados/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Feriado_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarFeriadoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _feriadoAppService.Obter((long)id); //_Feriadoservice.GetFeriados(new GetFeriadosInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarFeriadoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarFeriadoModalViewModel(new CriarOuEditarFeriado());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Feriados/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}