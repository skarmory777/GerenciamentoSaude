using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Sac;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class SacController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new SacViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Sac/Index.cshtml", viewModel);
        }
    }
}