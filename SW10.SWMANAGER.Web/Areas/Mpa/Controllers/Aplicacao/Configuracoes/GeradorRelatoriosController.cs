using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Configuracoes.GeradorRelatorios;
using SW10.SWMANAGER.Web.Controllers;

using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Configuracoes
{
    public class GeradorRelatoriosController : SWMANAGERControllerBase
    {
        // GET: Mpa/Agendamentos
        public ActionResult Index()
        {
            var viewModel = new GeradorRelatoriosViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/GeradorRelatorios/Index.cshtml", viewModel);
        }
    }
}