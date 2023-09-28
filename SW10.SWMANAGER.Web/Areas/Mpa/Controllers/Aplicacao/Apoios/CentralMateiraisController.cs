using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.CentralMateriais;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class CentralMateriaisController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new CentralMateriaisViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/CentralMateriais/Index.cshtml", viewModel);
        }
    }
}