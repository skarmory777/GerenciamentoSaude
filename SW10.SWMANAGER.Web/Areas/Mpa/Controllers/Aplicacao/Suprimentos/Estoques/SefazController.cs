using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class SefazController : Controller
    {
        public async Task<ActionResult> NotasPendente()
        {
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Sefaz/notas-pendentes.cshtml", null);
        }
    }
}