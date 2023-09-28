using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.RegrasConveniosParticulares;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class RegrasConveniosParticularesController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new RegrasConveniosParticularesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/RegrasConveniosParticulares/Index.cshtml", viewModel);
        }
    }
}