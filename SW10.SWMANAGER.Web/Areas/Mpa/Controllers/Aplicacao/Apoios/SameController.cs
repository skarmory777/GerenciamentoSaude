using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.Same;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class SameController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new SameViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/Same/Index.cshtml", viewModel);
        }
    }
}