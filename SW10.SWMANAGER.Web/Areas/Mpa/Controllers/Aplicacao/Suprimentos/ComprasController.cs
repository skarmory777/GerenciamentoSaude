using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Compras;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos
{
    public class ComprasController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new ComprasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Compras/Index.cshtml", viewModel);
        }
    }
}