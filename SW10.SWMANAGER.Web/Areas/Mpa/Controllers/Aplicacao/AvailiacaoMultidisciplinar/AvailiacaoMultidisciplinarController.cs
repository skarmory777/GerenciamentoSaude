namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.AvailiacaoMultidisciplinar
{
    using Abp.Dependency;
    using SW10.SWMANAGER.Web.Controllers;
    using System.Threading.Tasks;
    using System.Web.Mvc;

    public class AvailiacaoMultidisciplinarController : SWMANAGERControllerBase


    {
        private readonly IIocResolver iocResolver;

        public async Task<ActionResult> Index()
        {
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/AvailiacaoMultidisciplinar/index.cshtml",
                null);
        }

        public async Task<PartialViewResult> CriarOuEditarAvailiacaoMultidisciplinarConfig()
        {
            return this.PartialView(
                "~/Areas/Mpa/Views/Aplicacao/AvailiacaoMultidisciplinar/CriarOuEditarFormularioConfig.cshtml",
                null);
        }
    }
}