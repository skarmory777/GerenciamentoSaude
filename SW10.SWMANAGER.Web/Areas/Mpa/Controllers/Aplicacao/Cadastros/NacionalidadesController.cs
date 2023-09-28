using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Nacionalidades.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Nacionalidades;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class NacionalidadesController : SWMANAGERControllerBase
    {
        private readonly INacionalidadeAppService _nacionalidadeAppService;

        public NacionalidadesController(
            INacionalidadeAppService nacionalidadeAppService
            )
        {
            _nacionalidadeAppService = nacionalidadeAppService;
        }
        // GET: Mpa/Nacionalidade
        public ActionResult Index()
        {
            var model = new NacionalidadesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Nacionalidades/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Nacionalidade_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarNacionalidadeModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _nacionalidadeAppService.Obter((long)id); //_Nacionalidadeservice.GetNacionalidades(new GetNacionalidadesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarNacionalidadeModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarNacionalidadeModalViewModel(new CriarOuEditarNacionalidade());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Nacionalidades/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}