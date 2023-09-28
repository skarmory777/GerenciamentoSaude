using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.FaturamentoSUSInternacao;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos
{
    public class FaturamentoSUSInternacaoController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new FaturamentoSUSInternacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/FaturamentoSUSInternacao/Index.cshtml", viewModel);
        }
    }
}