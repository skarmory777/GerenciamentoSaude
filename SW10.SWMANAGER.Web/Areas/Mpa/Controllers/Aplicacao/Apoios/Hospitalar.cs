using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Hospitalar;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class HospitalarController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new HospitalarViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Hospitalar/Index.cshtml", viewModel);
        }
    }
}