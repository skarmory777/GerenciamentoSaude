using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.KitsExames.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Laboratorios.KitsExames;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Laboratorios
{
    public class KitsExamesController : SWMANAGERControllerBase
    {
        private readonly IKitExameAppService _kitExameAppService;
        private readonly IKitExameItemAppService _kitExameItemAppService;


        public KitsExamesController(
            IKitExameItemAppService kitExameItemAppService,
            IKitExameAppService kitExameAppService
            )
        {
            _kitExameItemAppService = kitExameItemAppService;
            _kitExameAppService = kitExameAppService;
        }

        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/KitsExames/index.cshtml", new KitsExamesViewModel());
        }

        public async Task<ActionResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarKitExameModalViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _kitExameAppService.Obter(id.Value);
                viewModel = new CriarOuEditarKitExameModalViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarKitExameModalViewModel(new KitExameDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/KitsExames/_CriarOuEditarModal.cshtml", viewModel);
        }

        public PartialViewResult SelecionarKitExameModal(long? id)
        {
            var viewModel = new SelecionarKitExameViewModel();
            viewModel.Id = id??0;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Laboratorios/KitsExames/_SelecionarKitExameItemModal.cshtml", viewModel);
        }


    }
}