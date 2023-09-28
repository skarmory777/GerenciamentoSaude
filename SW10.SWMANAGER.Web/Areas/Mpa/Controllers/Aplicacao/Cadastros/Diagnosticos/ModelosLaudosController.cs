using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Laudos.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Diagnosticos.ModelosLaudos;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Diagnosticos
{
    public class ModelosLaudosController : SWMANAGERControllerBase
    {
        private readonly IModeloLaudoAppService _modeloLaudoAppService;

        public ModelosLaudosController(
            IModeloLaudoAppService modeloLaudoAppService
            )
        {
            _modeloLaudoAppService = modeloLaudoAppService;
        }
        // GET: Mpa/Laudo
        public ActionResult Index()
        {
            var model = new ModeloLaudoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/ModelosLaudos/Index.cshtml", model);
        }

        //  [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Assistencial_LaudoMedico_Create, AppPermissions.Pages_Tenant_Assistencial_LaudoMedico_Edit)]
        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarModeloLaudoViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _modeloLaudoAppService.Obter((long)id);
                viewModel = new CriarOuEditarModeloLaudoViewModel(output);

                if (viewModel.LaudoGrupo == null)
                {
                    viewModel.LaudoGrupo = new ClassesAplicacao.Diagnosticos.Imagens.LaudoGrupo();
                    viewModel.LaudoGrupoId = 0;
                }
            }
            else
            {
                viewModel = new CriarOuEditarModeloLaudoViewModel(new ModeloLaudoDto());
                viewModel.LaudoGrupo = new ClassesAplicacao.Diagnosticos.Imagens.LaudoGrupo();
                viewModel.LaudoGrupoId = 0;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/ModelosLaudos/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}