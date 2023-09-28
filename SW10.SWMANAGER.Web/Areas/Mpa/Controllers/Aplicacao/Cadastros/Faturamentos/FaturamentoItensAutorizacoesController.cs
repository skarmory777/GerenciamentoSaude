#region Usings
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Autorizacoes;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Cadastros.FaturamentoItensAutorizacoes;
using SW10.SWMANAGER.Web.Controllers;
using System.Threading.Tasks;
using System.Web.Mvc;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Cadastros
{
    public class FaturamentoItensAutorizacoesController : SWMANAGERControllerBase
    {
        private readonly IFaturamentoItemAutorizacaoAppService _faturamentoItemAutorizacaoAppService;
        public FaturamentoItensAutorizacoesController(IFaturamentoItemAutorizacaoAppService faturamentoItemAutorizacaoAppService)
        {
            _faturamentoItemAutorizacaoAppService = faturamentoItemAutorizacaoAppService;
        }


        public ActionResult Index()
        {
            var model = new FaturamentoItemAutorizacaoViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/FaturamentoItensAutorizacoes/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            FaturamentoItemAutorizacaoViewModel viewModel = null;
            if (id.HasValue)
            {
                var faturamentoItemAutorizacao = await _faturamentoItemAutorizacaoAppService.Obter((long)id);

                if (faturamentoItemAutorizacao != null)
                {
                    viewModel = new FaturamentoItemAutorizacaoViewModel(faturamentoItemAutorizacao);
                }


            }
            else
            {
                viewModel = new FaturamentoItemAutorizacaoViewModel();
            }
            return View("~/Areas/Mpa/Views/Aplicacao/Cadastros/Faturamentos/FaturamentoItensAutorizacoes/_CriarOuEditarModal.cshtml", viewModel);
        }


    }
}