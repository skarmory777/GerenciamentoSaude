using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Configuracoes.Empresas.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using SW10.SWMANAGER.Sessions;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Financeiros.Relatorios;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Relatorios.Financeiro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Financeiros
{
    public class ContasPagarController : SWMANAGERControllerBase
    {
        private readonly IContasPagarAppService _contasPagarAppService;
        private readonly IUserAppService _userAppService;
        private readonly IEmpresaAppService _empresaAppService;
        private readonly ISessionAppService _sessionAppService;

        public ContasPagarController(
            IContasPagarAppService contasPagarAppService,
            IUserAppService userAppService,
            IEmpresaAppService empresaAppService,
            ISessionAppService sessionAppService
            )
        {
            _contasPagarAppService = contasPagarAppService;
            _userAppService = userAppService;
            _empresaAppService = empresaAppService;
            _sessionAppService = sessionAppService;
        }

        public ActionResult Index()
        {
            var model = new ContasPagarViewModel(new DocumentoDto());
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasPagar/Index.cshtml", model);
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var viewModel = new ContasPagarViewModel(new DocumentoDto())
            {
                LancamentosJson = JsonConvert.SerializeObject(new List<LancamentoDto>()),
                RateioJson = JsonConvert.SerializeObject(new List<DocumentoRateioIndex>())
            };

            if (id != null && id != 0)
            {
                var lancamentosIndex = new List<LancamentoIndex>();
                var rateiosIndex = new List<DocumentoRateioIndex>();
                var documento = await _contasPagarAppService.ObterPorLancamento((long)id).ConfigureAwait(false);
                viewModel = new ContasPagarViewModel(documento);

                #region Lista Lançamentos
                foreach (var item in documento.LancamentosDto)
                {
                    var lancamentoIndex = new LancamentoIndex();

                    lancamentoIndex.Id = item.Id;
                    lancamentoIndex.AnexoListaId = item.AnexoListaId;
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
                    lancamentoIndex.TotalLancamento = item.Total;

                    lancamentosIndex.Add(lancamentoIndex);
                }
                #endregion


                long idGrid = 0;
                foreach (var item in documento.DocumentosRateiosDto)
                {
                    var documentoRateioIndex = new DocumentoRateioIndex();

                    documentoRateioIndex.Id = item.Id;
                    documentoRateioIndex.CentroCustoId = item.CentroCustoId;
                    documentoRateioIndex.CentroCustoDescricao = string.Concat(item.CentroCusto.Codigo, " - ", item.CentroCusto.Descricao);
                    documentoRateioIndex.ContaAdministrativaId = item.ContaAdministrativaId;
                    documentoRateioIndex.ContaAdministrativaDescricao = string.Concat(item.ContaAdministrativa.Codigo, " - ", item.ContaAdministrativa.Descricao);
                    documentoRateioIndex.EmpresaId = item.EmpresaId;
                    documentoRateioIndex.EmpresaDescricao = string.Concat(item.Empresa.Codigo, " - ", item.Empresa.NomeFantasia);
                    documentoRateioIndex.Valor = item.Valor;
                    documentoRateioIndex.IsImposto = item.IsImposto;
                    documentoRateioIndex.Observacao = item.Observacao;
                    documentoRateioIndex.IdGrid = idGrid++;
                    rateiosIndex.Add(documentoRateioIndex);
                }

                viewModel.ValorTotalParcelas = documento.LancamentosDto.Sum(s => s.ValorLancamento);
                viewModel.ValorTotalRateio = documento.DocumentosRateiosDto.Sum(s => s.Valor);
                viewModel.LancamentosJson = JsonConvert.SerializeObject(lancamentosIndex);
                viewModel.RateioJson = JsonConvert.SerializeObject(rateiosIndex);
            }


            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/ContasPagar/_CriarOuEditarModal.cshtml", viewModel);
        }

        public ContentResult VisualizarRptContaPagarResumidoPDF(RptContaPagarViewModel input)
        {
            var msg = string.Empty;
            try
            {
                var empresa = new EmpresaDto();
                var usuarioEmpresas = Task.Run(() => _userAppService.GetUserEmpresas(AbpSession.UserId.Value)).Result;
                if (input.EmpresaId.HasValue && input.EmpresaId.Value > 0)
                {
                    empresa = Task.Run(() => _empresaAppService.Obter(input.EmpresaId.Value)).Result;
                }
                else
                {
                    empresa = usuarioEmpresas.Items.FirstOrDefault();
                }
                var usuario = Task.Run(() => _userAppService.GetUser()).Result;
                var loginInformations = Task.Run(() => _sessionAppService.GetCurrentLoginInformations()).Result;
                var dados = new FiltroModel();
                dados.Titulo = "Relatório de contas a" + (input.IsCredito ? " receber " : " pagar ") + (input.TipoRel == 1 ? "resumido" : "detalhado");
                dados.NomeHospital = empresa.NomeFantasia;
                dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                dados.StartDate = input.StartDate.ToString("dd/MM/yyyy");
                dados.EndDate = input.EndDate.ToString("dd/MM/yyyy");
                // Obtido do ASPX
                ScriptManager scriptManager = new ScriptManager();
                ReportViewer reportViewer = new ReportViewer();

                reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Financeiro\ContasPagar\Resumido.rdlc");
                //dados.Titulo += " por data do atendimento";

                if (dados != null)
                {
                    //parâmetros para o relatório
                    ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                    ReportParameter nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                    ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                    ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);
                    ReportParameter _startDate = new ReportParameter("StartDate", dados.StartDate);
                    ReportParameter _endDate = new ReportParameter("EndDate", dados.EndDate);

                    reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                            nomeHospital,
                            nomeUsuario,
                            titulo,
                            dataHora,
                            _startDate,
                            _endDate
                        });

                    //fonte de dados para o relatório - datasource

                    var list = Task.Run(() => _contasPagarAppService.ListarContaPagarDetalhadoReport(new VWRptContaPagarInput
                    {
                        EmpresaId = input.EmpresaId,
                        EndDate = input.EndDate,
                        MeioPagamentoId = input.MeioPagamentoId,
                        PessoaId = input.PessoaId,
                        Situacao = input.Situacao,
                        SituacaoNotaFiscal = input.SituacaoNotaFiscal,
                        StartDate = input.StartDate,
                        TipoData = input.TipoData,
                        TipoDocumentoId = input.TipoDocumentoId,
                        TipoPessoaId = input.TipoPessoaId,
                        TipoRel = input.TipoRel,
                        IsCredito = input.IsCredito

                    })).Result;
                    var listDto = list.Items.ToList();
                    FinanceiroRelatorioDS relDS = new FinanceiroRelatorioDS();
                    DataTable tabela = ConvertToDataTable(listDto, relDS.Tables["ContaPagarDetalhado"]);

                    // Logotipo
                    if (tabela.Rows.Count > 0)
                    {
                        tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                    }
                    // fim - logotipo

                    ReportDataSource dataSource = new ReportDataSource("Resumido", tabela);

                    reportViewer.LocalReport.DataSources.Add(dataSource);

                    scriptManager.RegisterPostBackControl(reportViewer);

                    // Gerando PDF
                    string mimeType = "application/pdf"; //string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";

                    string[] streamIds;
                    Warning[] warnings;
                    byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                    var absPath = string.Concat(Server.MapPath("~"), @"temp\");
                    var path = string.Empty;
                    var file = string.Empty;
                    var pathReturn = string.Empty;
                    file = string.Concat("ContaPagarReceberResumido-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                    path = string.Concat(absPath, file);
                    pathReturn = Url.Content("~/temp/" + file);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                    fs.Close();

                    reportViewer.LocalReport.Refresh();

                    Response.Headers.Add("Content-Disposition", string.Format("inline; filename=ContaPagarReceberResumido-{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
                    msg = pathReturn;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                throw new Exception(e.Message.ToString(), e);
            }
            return Content(msg);
        }

        public ContentResult VisualizarRptContaPagarDetalhadoPDF(RptContaPagarViewModel input)
        {
            var msg = string.Empty;
            try
            {
                var empresa = new EmpresaDto();
                var usuarioEmpresas = Task.Run(() => _userAppService.GetUserEmpresas(AbpSession.UserId.Value)).Result;
                if (input.EmpresaId.HasValue && input.EmpresaId.Value > 0)
                {
                    empresa = Task.Run(() => _empresaAppService.Obter(input.EmpresaId.Value)).Result;
                }
                else
                {
                    empresa = usuarioEmpresas.Items.FirstOrDefault();
                }
                var usuario = Task.Run(() => _userAppService.GetUser()).Result;
                var loginInformations = Task.Run(() => _sessionAppService.GetCurrentLoginInformations()).Result;
                var dados = new FiltroModel();
                dados.Titulo = "Relatório de contas a" + (input.IsCredito ? " receber " : " pagar ") + (input.TipoRel == 1 ? "resumido" : "detalhado");
                dados.NomeHospital = empresa.NomeFantasia;
                dados.NomeUsuario = string.Concat(loginInformations.User.Name, " ", loginInformations.User.Surname);
                dados.DataHora = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss");
                dados.StartDate = input.StartDate.ToString("dd/MM/yyyy");
                dados.EndDate = input.EndDate.ToString("dd/MM/yyyy");
                // Obtido do ASPX
                ScriptManager scriptManager = new ScriptManager();
                ReportViewer reportViewer = new ReportViewer();

                reportViewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Financeiro\ContasPagar\ContaPagarDetalhado.rdlc");
                //dados.Titulo += " por data do atendimento";

                if (dados != null)
                {
                    //parâmetros para o relatório
                    ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                    ReportParameter nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                    ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                    ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);
                    ReportParameter _startDate = new ReportParameter("StartDate", dados.StartDate);
                    ReportParameter _endDate = new ReportParameter("EndDate", dados.EndDate);

                    reportViewer.LocalReport.SetParameters(new ReportParameter[] {
                            nomeHospital,
                            nomeUsuario,
                            titulo,
                            dataHora,
                            _startDate,
                            _endDate
                        });

                    //fonte de dados para o relatório - datasource

                    var list = Task.Run(() => _contasPagarAppService.ListarContaPagarDetalhadoReport(new VWRptContaPagarInput
                    {
                        EmpresaId = input.EmpresaId,
                        EndDate = input.EndDate,
                        MeioPagamentoId = input.MeioPagamentoId,
                        PessoaId = input.PessoaId,
                        Situacao = input.Situacao,
                        SituacaoNotaFiscal = input.SituacaoNotaFiscal,
                        StartDate = input.StartDate,
                        TipoData = input.TipoData,
                        TipoDocumentoId = input.TipoDocumentoId,
                        TipoPessoaId = input.TipoPessoaId,
                        TipoRel = input.TipoRel,
                        IsCredito = input.IsCredito

                    })).Result;
                    var listDto = list.Items.ToList();
                    FinanceiroRelatorioDS relDS = new FinanceiroRelatorioDS();
                    DataTable tabela = ConvertToDataTable(listDto, relDS.Tables["ContaPagarDetalhado"]);

                    // Logotipo
                    if (tabela.Rows.Count > 0)
                    {
                        tabela.Rows[0]["Logotipo"] = empresa.Logotipo;
                    }
                    // fim - logotipo

                    ReportDataSource dataSource = new ReportDataSource("ContaPagarDetalhado", tabela);

                    reportViewer.LocalReport.DataSources.Add(dataSource);

                    scriptManager.RegisterPostBackControl(reportViewer);

                    // Gerando PDF
                    string mimeType = "application/pdf"; //string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";

                    string[] streamIds;
                    Warning[] warnings;
                    byte[] pdfBytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                    //if (System.IO.File.Exists(@"C:\Temp\SaldoProduto.pdf"))
                    var absPath = string.Concat(Server.MapPath("~"), @"temp\");
                    var path = string.Empty;
                    var file = string.Empty;
                    var pathReturn = string.Empty;
                    file = string.Concat("ContaPagarReceberDetalhado-", DateTime.Now.ToString("yyyyMMddHHmmss"), ".pdf");
                    path = string.Concat(absPath, file);
                    pathReturn = Url.Content("~/temp/" + file);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    FileStream fs = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
                    fs.Write(pdfBytes, 0, pdfBytes.Length);
                    fs.Close();

                    reportViewer.LocalReport.Refresh();

                    Response.Headers.Add("Content-Disposition", string.Format("inline; filename=ContaPagarReceberDetalhado-{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")));
                    msg = pathReturn;
                }
            }
            catch (Exception e)
            {
                msg = e.Message.ToString();
                throw new Exception(e.Message.ToString(), e);
            }
            return Content(msg);
        }

        public async Task<ActionResult> VisualizarNotaFiscal(int lancamentoId)
        {
            var lancamentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<Lancamento, long>>();
            var estoquePreMovimentoRepository = IocManager.Instance.ResolveAsDisposable<IRepository<EstoquePreMovimento, long>>();
            var lancamento = await lancamentoRepository.Object
                                .GetAll().Include(x => x.Documento).AsNoTracking()
                                .SingleOrDefaultAsync(x => x.Id == lancamentoId)
                                .ConfigureAwait(false);

            var estoquePreMovimento = await estoquePreMovimentoRepository.Object
                                        .GetAll().Where(x => x.Chave != null).AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Documento == lancamento.Documento.Numero)
                                        .ConfigureAwait(false);

            if (estoquePreMovimento != null)
                return RedirectToAction("CriarOuEditarModal", "PreMovimentos", new { id = estoquePreMovimento.Id });

            return HttpNotFound();
        }

        public ActionResult ContasPagarRelatorio(RptContaPagarViewModel model)
        {
            return View("~/Areas/Mpa/Views/Aplicacao/Financeiros/Relatorios/ContasPagar/Index.cshtml", model);
        }

        [UnitOfWork(false)]
        public ActionResult GerarRelatorio(DateTime dataInicio, DateTime dataFim, bool isCredito, long? empresaId, long? pessoaId, long? situacaoLancamentoId)
        {
            using (var contasPagarAppService = IocManager.Instance.ResolveAsDisposable<IContasPagarAppService>())
            {
                return File(contasPagarAppService.Object.GerarRelatorio(new RelatorioContasApagarDto()
                {
                    DataFim = dataFim,
                    DataInicio = dataInicio,
                    IsCredito = isCredito,
                    EmpresaId = empresaId,
                    PessoaId = pessoaId,
                    SituacaoLancamentoId = situacaoLancamentoId
                }), "application/pdf", $"relatorio-contas-pagar.pdf");
            }
        }

        [UnitOfWork(false)]
        public ActionResult GerarRelatorioQuitacao(DateTime dataInicio, DateTime dataFim, bool isCredito, long? empresaId, long? pessoaId, long? situacaoLancamentoId)
        {
            using (var contasPagarAppService = IocManager.Instance.ResolveAsDisposable<IContasPagarAppService>())
            {
                return File(contasPagarAppService.Object.GerarRelatorioQuitacao(new RelatorioContasApagarDto()
                {
                    DataFim = dataFim,
                    DataInicio = dataInicio,
                    IsCredito = isCredito,
                    EmpresaId = empresaId,
                    PessoaId = pessoaId,
                    SituacaoLancamentoId = situacaoLancamentoId
                }), "application/pdf", $"relatorio-contas-pagar-quitacao.pdf");
            }
        }

        [UnitOfWork(false)]
        public ActionResult GerarRelatorioGroupNome
            (DateTime dataInicio, DateTime dataFim, bool isCredito, long? empresaId, long? pessoaId, long? situacaoLancamentoId)
        {
            using (var contasPagarAppService = IocManager.Instance.ResolveAsDisposable<IContasPagarAppService>())
            {
                return File(contasPagarAppService.Object.GerarRelatorioGroupNome(new RelatorioContasApagarDto()
                {
                    DataFim = dataFim,
                    DataInicio = dataInicio,
                    IsCredito = isCredito,
                    EmpresaId = empresaId,
                    PessoaId = pessoaId,
                    SituacaoLancamentoId = situacaoLancamentoId
                }), "application/pdf", $"relatorio-contas-pagar-fornecedor.pdf");
            }
        }
    }
}