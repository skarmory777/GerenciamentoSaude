using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Clinico;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class ClinicoController : SWMANAGERControllerBase
    {
        // GET: Mpa/AtendimentoClinico
        public ActionResult Index()
        {
            var viewModel = new ClinicoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/Clinico/Index.cshtml", viewModel);
        }
    }
}