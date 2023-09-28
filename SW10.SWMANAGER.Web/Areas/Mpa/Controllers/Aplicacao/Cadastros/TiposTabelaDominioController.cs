using Abp.Web.Mvc.Authorization;
using SW10.SWMANAGER.Authorization;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.TiposTabelaDominio.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.TiposTabelaDominio;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class TiposTabelaDominioController : SWMANAGERControllerBase
    {
        private readonly ITipoTabelaDominioAppService _tipoTabelaDominioAppService;

        public TiposTabelaDominioController(
            ITipoTabelaDominioAppService tipoTabelaDominioAppService
            )
        {
            _tipoTabelaDominioAppService = tipoTabelaDominioAppService;
        }
        // GET: Mpa/TipoTabelaDominio
        public ActionResult Index()
        {
            var model = new TiposTabelaDominioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposTabelaDominio/Index.cshtml", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Create, AppPermissions.Pages_Tenant_Cadastros_DominioTiss_TiposTabelaDominio_Edit)]
        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            CriarOuEditarTipoTabelaDominioModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _tipoTabelaDominioAppService.Obter((long)id); //_TiposTabelaDominioervice.GetTiposTabelaDominio(new GetTiposTabelaDominioInput());//.Items.Where(m => m.Id == id).FirstOrDefault(); // _userAppService.GetUserForEdit(new NullableIdDto<long> { Id = id });
                                                                                 //var result = output.Items.Where(m => m.Id == id).FirstOrDefault();
                viewModel = new CriarOuEditarTipoTabelaDominioModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarTipoTabelaDominioModalViewModel(new CriarOuEditarTipoTabelaDominio());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/TiposTabelaDominio/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}