using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Farmacia;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos
{
    public class FarmaciaController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new FarmaciaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Farmacia/Index.cshtml", viewModel);
        }
    }
}