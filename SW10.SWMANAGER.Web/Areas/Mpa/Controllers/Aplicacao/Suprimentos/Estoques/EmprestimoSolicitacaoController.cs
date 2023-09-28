using Abp.Dependency;
using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Web.Controllers;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos.Estoques
{
    public class EmprestimoSolicitacaoController : SWMANAGERControllerBase
    {
        [HttpGet]
        public ActionResult Recebimento()
        {
            var viewModel = new PreMovimentoViewModel()
            {
                EstTipoOperacaoId = (long)EnumTipoOperacao.Entrada,
                EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Entrada
            };

            ViewBag.PageTitle = "SolicitacaoRecebimento";

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Solicitacao/Index.cshtml", viewModel);
        }

        [HttpGet]
        public ActionResult Concessao()
        {
            var viewModel = new PreMovimentoViewModel()
            {
                EstTipoOperacaoId = (long)EnumTipoOperacao.Saida,
                EstTipoMovimentoId = (long)EnumTipoMovimento.Emprestimo_Saida
            };

            ViewBag.PageTitle = "SolicitacaoConcessao";

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Solicitacao/Index.cshtml", viewModel);
        }

        [HttpGet]
        public async Task<ActionResult> CriarOuEditarModal(long? id, long? estTipoOperacaoId = (long)EnumTipoOperacao.Entrada)
        {
            var viewModel = new CriarOuEditarPreMovimentoModalViewModel(new PreMovimentoViewModel())
            {
                Movimento = DateTime.Now,
                EstoqueEmprestimo = new EstoqueEmprestimoDto(),
                EstTipoOperacaoId = estTipoOperacaoId
            };

            if (id.HasValue)
            {
                using (var estoquePreMovimentoItemAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoItemAppService>())
                using (var preMovimentoAppService = IocManager.Instance.ResolveAsDisposable<IEstoquePreMovimentoAppService>())
                {
                    var output = await preMovimentoAppService.Object.Obter(id.Value).ConfigureAwait(false);
                    var itens = await estoquePreMovimentoItemAppService.Object.ObterItensPorPreMovimento(id.Value).ConfigureAwait(false);

                    viewModel = new CriarOuEditarPreMovimentoModalViewModel(output)
                    {
                        PermiteConfirmacaoEntrada = await preMovimentoAppService.Object.PermiteConfirmarEntrada(output).ConfigureAwait(false),
                        Itens = JsonConvert.SerializeObject(itens)
                    };
                }
            }

            return View("~/Areas/Mpa/Views/Aplicacao/Suprimentos/Estoques/Emprestimos/Solicitacao/CriarOuEditarModal/_CriarOuEditarModal.cshtml", viewModel);
        }
    }
}