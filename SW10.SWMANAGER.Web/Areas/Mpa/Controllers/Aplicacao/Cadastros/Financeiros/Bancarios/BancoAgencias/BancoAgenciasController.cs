using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Financeiro.Bancarios.BancoAgencias;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.Financeiros.Bancarios.BancoAgencias;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros.Financeiros.Bancarios.BancoAgencias
{
    public class BancoAgenciasController : SWMANAGERControllerBase
    {

        private readonly IBancoAgenciasAppService _bancoAgenciasAppService;

        public BancoAgenciasController(IBancoAgenciasAppService bancoAgenciasAppService)
        {
            _bancoAgenciasAppService = bancoAgenciasAppService;
        }

        // GET: Mpa/TipoLocalChamada
        public ActionResult Index()
        {
            var model = new BancoAgenciasViewModel(new BancoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/BancoAgencias/Index.cshtml", model);
        }

        public async Task<PartialViewResult> CriarOuEditarModal(long? id)
        {
            BancoAgenciasViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new BancoAgenciasViewModel(new BancoDto());
            }
            else
            {
                var bancoDto = await _bancoAgenciasAppService.Obter((long)id);

                viewModel = new BancoAgenciasViewModel(bancoDto);
            }

            return PartialView("~/Areas/Mpa/Views/Aplicacao/Cadastros/Financeiros/Bancarios/BancoAgencias/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}