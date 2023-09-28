using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AtendimentoExames;
using SW10.SWMANAGER.Web.Controllers;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class AtendimentoExamesController : SWMANAGERControllerBase
    {
        // GET: Mpa/Exames
        public ActionResult Index()
        {
            var viewModel = new AtendimentoExamesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AtendimentoExames/Index.cshtml", viewModel);
        }
    }
}