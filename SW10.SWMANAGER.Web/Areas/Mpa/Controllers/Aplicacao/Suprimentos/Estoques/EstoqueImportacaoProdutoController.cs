using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class EstoqueImportacaoProdutoController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/EstoqueImportacaoProduto/Index.cshtml");
        }
    }
}