using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.TalaoCheques;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros.Bancarios.TalaoCheque;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Financeiros.Bacarios.TalaoCheque
{
    public class TalaoChequeController : SWMANAGERControllerBase
    {

        private readonly ITalaoChequeAppService _talaoChequeAppService;

        public TalaoChequeController(ITalaoChequeAppService talaoChequeAppService)
        {
            _talaoChequeAppService = talaoChequeAppService;
        }

        // GET: Mpa/TipoLocalChamada
        public ActionResult Index()
        {
            var model = new TalaoChequeViewModel(new TalaoChequeDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/TalaoCheque/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            TalaoChequeViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new TalaoChequeViewModel(new TalaoChequeDto());
            }
            else
            {
                var tipoLocalChamadaDto = await _talaoChequeAppService.Obter((long)id);

                viewModel = new TalaoChequeViewModel(tipoLocalChamadaDto);
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/TalaoCheque/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}