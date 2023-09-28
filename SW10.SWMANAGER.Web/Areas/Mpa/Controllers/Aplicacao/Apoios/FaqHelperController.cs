namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Apoios
{
    using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Apoios.FaqHelper;
    using SW10.SWMANAGER.Web.Controllers;
    using System.Web.Mvc;
    public class FaqHelperController : SWMANAGERControllerBase
    {
        // GET: Mpa/DisparoDeMensagem/Index
        public ActionResult Index()
        {
            var viewModel = new IndexFaqHelperViewModel();
            return this.View("~/Areas/Mpa/Views/Aplicacao/Apoios/FaqHelper/index.cshtml", viewModel);
        }

        
    }
    
}