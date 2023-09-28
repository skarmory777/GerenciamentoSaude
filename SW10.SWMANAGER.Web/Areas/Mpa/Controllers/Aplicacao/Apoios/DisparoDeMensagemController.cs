namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    using SW10.SWMANAGER.Web.Controllers;
    using System.Web.Mvc;
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.DisparoDeMensagem;
    public class DisparoDeMensagemController : SWMANAGERControllerBase
    {
        // GET: Mpa/DisparoDeMensagem/Index
        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/DisparoDeMensagem/index.cshtml", new IndexDisparoDeMensagemViewModel());
        }

        // GET: Mpa/DisparoDeMensagem/IndexCriarOuEditar
        public ActionResult IndexCriarOuEditar(long? id)
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Apoios/DisparoDeMensagem/IndexCriarOuEditar.cshtml", new IndexCriarOuEditarDisparoDeMensagemViewModel {Id = id});
        }
    }
}