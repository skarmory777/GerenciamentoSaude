using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.LavanderiaRouparia;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    public class LavanderiaRoupariaController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new LavanderiaRoupariaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/LavanderiaRouparia/Index.cshtml", viewModel);
        }
    }
}