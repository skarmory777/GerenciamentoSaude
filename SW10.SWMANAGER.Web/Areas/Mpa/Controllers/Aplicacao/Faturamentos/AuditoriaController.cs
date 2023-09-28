using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Auditoria;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class AuditoriaController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new AuditoriaViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Auditoria/Index.cshtml", viewModel);
        }
    }
}