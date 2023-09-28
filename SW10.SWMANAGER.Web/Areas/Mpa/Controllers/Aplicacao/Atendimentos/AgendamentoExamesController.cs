using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoExames;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Atendimentos
{
    public class AgendamentoExamesController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new AgendamentoExamesViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Atendimentos/AgendamentoExames/Index.cshtml", viewModel);
        }
    }
}