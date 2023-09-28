using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Regioes.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Regioes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class RegioesController : SWMANAGERControllerBase
    {
        private readonly IRegiaoAppService _regiaoAppService;

        public RegioesController(
            IRegiaoAppService regiaoAppService
            )
        {
            _regiaoAppService = regiaoAppService;
        }
        // GET: Mpa/Regiao
        public ActionResult Index()
        {
            var model = new RegioesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Regioes/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Create, AppPermissions.Pages_Tenant_Cadastros_CadastrosGlobais_Regioes_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarRegiaoModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _regiaoAppService.Obter((long)id); //_Regioeservice.GetRegioes(new GetRegioesInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarRegiaoModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarRegiaoModalViewModel(new CriarOuEditarRegiao());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Regioes/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}