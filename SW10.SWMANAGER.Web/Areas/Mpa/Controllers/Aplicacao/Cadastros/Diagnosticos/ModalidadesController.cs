using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Diagnosticos.Modalidades.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Diagnosticos.Modalidades;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Diagnosticos
{
    public class ModalidadesController : SWMANAGERControllerBase
    {
        private readonly IModalidadeAppService _modalidadeAppService;

        public ModalidadesController(
            IModalidadeAppService modalidadeAppService
            )
        {
            _modalidadeAppService = modalidadeAppService;
        }
        // GET: Mpa/Modalidade
        public ActionResult Index()
        {
            var model = new ModalidadeViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/Modalidades/Index.cshtml", model);
        }

        public async Task<PartialViewResult> _CriarOuEditarModal(long? id)
        {
            CriarOuEditarModalidadeViewModel viewModel;
            if (id.HasValue)
            {
                var output = await _modalidadeAppService.Obter((long)id);
                viewModel = new CriarOuEditarModalidadeViewModel(output);
            }
            else
            {
                viewModel = new CriarOuEditarModalidadeViewModel(new ModalidadeDto());
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Diagnosticos/Modalidades/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}