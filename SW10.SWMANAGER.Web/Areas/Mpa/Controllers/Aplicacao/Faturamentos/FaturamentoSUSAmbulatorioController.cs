using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.FaturamentoSUSAmbulatorio;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoSUSAmbulatorioController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new FaturamentoSUSAmbulatorioViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturamentoSUSAmbulatorio/Index.cshtml", viewModel);
        }
    }
}