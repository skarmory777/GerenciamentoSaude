using Abp.Dependency;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Inventarios;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class InventariosController : Controller // Web.Controllers.SWMANAGERControllerBase
    {

        public InventariosController()
        {
        }

        public async Task<ActionResult> Index()
        {
            var model = new PreMovimentoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Inventarios/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var viewModel = new InventarioViewModel();

            if (id.HasValue) //edição
            {
                using (var inventarioAppService = IocManager.Instance.ResolveAsDisposable<IInventarioAppService>())
                {
                    var inventarioDto = await inventarioAppService.Object.Obter((long)id).ConfigureAwait(false);
                    if (inventarioDto != null)
                    {
                        viewModel.Id = inventarioDto.Id;
                        viewModel.Codigo = inventarioDto.Codigo;
                        viewModel.DataInventario = inventarioDto.DataInventario;
                        viewModel.StatusInventarioId = inventarioDto.StatusInventarioId;
                        viewModel.Status = inventarioDto.Status;
                        viewModel.EstoqueDescricao = inventarioDto.EstoqueDescricao;
                    }
                }
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Inventarios/_CriarOuEditarModal.cshtml", viewModel);
        }

    }
}