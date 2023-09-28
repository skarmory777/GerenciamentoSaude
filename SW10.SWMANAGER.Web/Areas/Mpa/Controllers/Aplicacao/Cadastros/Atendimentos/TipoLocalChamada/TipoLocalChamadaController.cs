using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.PainelSenhas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Atendimentos.TiposLocalChamada;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Atendimentos.TipoLocalChamada;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Atendimentos.TipoLocalChamada
{
    public class TipoLocalChamadaController : SWMANAGERControllerBase
    {

        private readonly ITipoLocalChamadaCoreAppService _tipoLocalChamadaAppService;

        public TipoLocalChamadaController(ITipoLocalChamadaCoreAppService tipoLocalChamadaAppService)
        {
            _tipoLocalChamadaAppService = tipoLocalChamadaAppService;
        }

        // GET: Mpa/TipoLocalChamada
        public ActionResult Index()
        {
            var model = new TipoLocalChamadaViewModel(new TipoLocalChamadaDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/TipoLocalChamada/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            TipoLocalChamadaViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new TipoLocalChamadaViewModel(new TipoLocalChamadaDto());
            }
            else
            {
                var tipoLocalChamadaDto = await _tipoLocalChamadaAppService.Obter((long)id);

                viewModel = new TipoLocalChamadaViewModel(tipoLocalChamadaDto);
            }



            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Atendimentos/TipoLocalChamada/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}