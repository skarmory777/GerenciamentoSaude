using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Diagnosticos.Laudos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Diagnosticos
{
    public class LaudoGruposController : SWMANAGERControllerBase
    {
        private readonly ILaudoGrupoAppService _modeloLaudoAppService;

        public LaudoGruposController(
            ILaudoGrupoAppService modeloLaudoAppService
            )
        {
            _modeloLaudoAppService = modeloLaudoAppService;
        }
        // GET: Mpa/Laudo
        public ActionResult Index()
        {
            var model = new LaudoGrupoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/LaudoGrupos/Index.cshtml", model);
        }

        //  [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_LaudoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_LaudoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarLaudoGrupoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _modeloLaudoAppService.Obter((long)id);
                viewModel = new CriarOuEditarLaudoGrupoViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarLaudoGrupoViewModel(new LaudoGrupoDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/LaudoGrupos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}