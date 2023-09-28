using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.FaturamentoConveniosParticular;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoConveniosParticularController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new FaturamentoConveniosParticularViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturamentoConveniosParticular/Index.cshtml", viewModel);
        }
    }
}