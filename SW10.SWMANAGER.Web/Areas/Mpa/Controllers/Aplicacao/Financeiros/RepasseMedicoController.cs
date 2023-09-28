using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.RepasseMedico;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class RepasseMedicoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new RepasseMedicoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/RepasseMedico/Index.cshtml", viewModel);
        }
    }
}