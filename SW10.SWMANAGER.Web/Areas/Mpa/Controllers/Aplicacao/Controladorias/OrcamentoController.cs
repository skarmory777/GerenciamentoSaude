using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Controladorias.Orcamento;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Controladorias
{
    public class OrcamentoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new OrcamentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Controladorias/Orcamento/Index.cshtml", viewModel);
        }
    }
}