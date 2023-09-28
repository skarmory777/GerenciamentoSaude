using Newtonsoft.Json;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Interface;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.Relatorios;
using SW10.SWMANAGER.Web.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class ContasReceberController : SWMANAGERControllerBase
    {
        private readonly IContasReceberAppService _contasPagarAppService;
        private readonly IDocumentoAppService _documentoAppService;

        public ContasReceberController(IContasReceberAppService contasPagarAppService, IDocumentoAppService documentoAppService)
        {
            _contasPagarAppService = contasPagarAppService;
            _documentoAppService = documentoAppService;
        }

        public ActionResult Index()
        {
            var model = new ContasPagarViewModel(new DocumentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasReceber/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            ContasPagarViewModel viewModel = null;
            if (id == null || id == 0)
            {
                viewModel = new ContasPagarViewModel(new DocumentoDto());
                viewModel.LancamentosJson = JsonConvert.SerializeObject(new List<LancamentoDto>());
                viewModel.RateioJson = JsonConvert.SerializeObject(new List<DocumentoRateioIndex>());
            }
            else
            {
                var documento = await _documentoAppService.ObterPorLancamento((long)id).ConfigureAwait(false);

                viewModel = new ContasPagarViewModel(documento);
                List<LancamentoIndex> lancamentosIndex = new List<LancamentoIndex>();
                List<DocumentoRateioIndex> rateiosIndex = new List<DocumentoRateioIndex>();

                #region Lista Lançamentos

                foreach (var item in documento.LancamentosDto)
                {
                    var lancamentoIndex = new LancamentoIndex();

                    lancamentoIndex.Id = item.Id;
                    lancamentoIndex.AnoCompetencia = item.AnoCompetencia;
                    lancamentoIndex.CodigoBarras = item.CodigoBarras;
                    lancamentoIndex.CorLancamentoFundo = item.SituacaoLancamento.CorLancamentoFundo;
                    lancamentoIndex.CorLancamentoLetra = item.SituacaoLancamento.CorLancamentoLetra;
                    lancamentoIndex.DataLancamento = item.DataLancamento;
                    lancamentoIndex.DataVencimento = item.DataVencimento;
                    lancamentoIndex.Juros = item.Juros;
                    lancamentoIndex.LinhaDigitavel = item.LinhaDigitavel;
                    lancamentoIndex.MesCompetencia = item.MesCompetencia;
                    lancamentoIndex.Multa = item.Multa;
                    lancamentoIndex.NossoNumero = item.NossoNumero;
                    lancamentoIndex.Parcela = item.Parcela;
                    lancamentoIndex.SituacaoDescricao = item.SituacaoDescricao;
                    lancamentoIndex.SituacaoLancamentoId = item.SituacaoLancamentoId;
                    lancamentoIndex.ValorAcrescimoDecrescimo = item.ValorAcrescimoDecrescimo;
                    lancamentoIndex.ValorLancamento = item.ValorLancamento;
                    lancamentoIndex.IsSelecionado = item.IsSelecionado;
                    lancamentoIndex.IdGrid = item.IdGrid;

                    lancamentosIndex.Add(lancamentoIndex);
                }

                #endregion


                long idGrid = 0;
                foreach (var item in documento.DocumentosRateiosDto)
                {
                    var documentoRateioIndex = new DocumentoRateioIndex
                    {
                        Id = item.Id,
                        CentroCustoId = item.CentroCustoId,
                        CentroCustoDescricao = string.Concat(item.CentroCusto.Codigo, " - ", item.CentroCusto.Descricao),
                        ContaAdministrativaId = item.ContaAdministrativaId,
                        ContaAdministrativaDescricao = string.Concat(item.ContaAdministrativa.Codigo, " - ", item.ContaAdministrativa.Descricao),
                        EmpresaId = item.EmpresaId,
                        EmpresaDescricao = string.Concat(item.Empresa.Codigo, " - ", item.Empresa.NomeFantasia),
                        Valor = item.Valor,
                        IsImposto = item.IsImposto,
                        Observacao = item.Observacao,
                        IdGrid = idGrid++
                    };
                    rateiosIndex.Add(documentoRateioIndex);
                }

                viewModel.ValorTotalParcelas = documento.LancamentosDto.Sum(s => s.ValorLancamento);
                viewModel.ValorTotalRateio = documento.DocumentosRateiosDto.Sum(s => s.Valor);


                viewModel.LancamentosJson = JsonConvert.SerializeObject(lancamentosIndex);
                viewModel.RateioJson = JsonConvert.SerializeObject(rateiosIndex);
            }


            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasReceber/_CriarOuEditarModal.cshtml", viewModel);
        }

        public ActionResult ContasReceberRelatorio(RptContaPagarViewModel model)
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/Relatorios/ContasReceber/Index.cshtml", model);
        }

        public ActionResult AbrirModalContasRecebidas(long quitacaoId)
        {
            var quitacaoDto = new QuitacaoDto();
            quitacaoDto.Id = quitacaoId;
            var viewModel = new QuitacaoViewModel(quitacaoDto); ;
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasReceber/ContasRecebidas/_ContasRecebidasModal.cshtml", viewModel);
        }

    }
}