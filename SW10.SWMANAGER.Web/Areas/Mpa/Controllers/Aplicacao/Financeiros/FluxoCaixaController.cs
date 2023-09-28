using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.FluxoCaixa;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class FluxoCaixaController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new FluxoCaixaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/FluxoCaixa/Index.cshtml", viewModel);
        }
    }
}