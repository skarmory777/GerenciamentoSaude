using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Parametrizacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers
{
    public class ParametrizacoesController : SWMANAGERControllerBase
    {
        public ActionResult Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Configuracoes/Parametrizacoes/Index.cshtml", null);

        }
    }
}