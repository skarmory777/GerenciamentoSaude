#region Usings
using Abp.Dependency;
using Microsoft.Reporting.WebForms;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Guias;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasMedicas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios.Guias;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.SolicitacaoInternacao;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Relatorios.Faturamento.Guias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos.GuiaConsulta
{
    public class FatGuiasConsultaController : SWMANAGERControllerBase
    {

        List<FaturamentoContaItemDto> _itensGuiaPrincipal = new List<FaturamentoContaItemDto>();

        // NOVO MODELO

        public async Task<ActionResult> GuiaConsultaPdf(long atendimentoId, long? contaId)
        {
            try
            {
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                using (var contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                {
                    var dados = new GuiaSpsadtModel();

                    if (contaId.HasValue)
                    {


                        var conta = await contaMedicaAppService.Object.ObterReportModel((long)contaId);

                        // Itens da conta
                        ListarFaturamentoContaItensInput listarItensInput = new ListarFaturamentoContaItensInput
                        {
                            Filtro = conta.Id.ToString()
                        };
                        var contaItensPaged = await contaItemAppService.Object.ListarPorConta(listarItensInput);
                        var contaItens = contaItensPaged.Items as List<FaturamentoContaItemDto>;
                        int totalItens = contaItens.Count;
                    }

                    var atendimento = await atendimentoAppService.Object.Obter((long)atendimentoId);
                    dados.LerAtendimento(atendimento);

                    // Guia principal
                    Guias relDS = new Guias();
                    DataTable tabela = this.ConvertToDataTable(new List<string>(), relDS.Tables["Consulta"]);//precisa?

                    // Logotipo
                    DataRow row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Empresa.Logotipo;
                    tabela.Rows.Add(row);
                    // fim - logotipo

                    ReportDataSource dataSource = new ReportDataSource("Consulta", tabela);
                    ReportViewer viewer = new ReportViewer();
                    viewer.LocalReport.DataSources.Add(dataSource);
                    ScriptManager scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(viewer);
                    viewer.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Consulta\guia_consulta.rdlc");

                    // Preenchendo parametros
                    ReportParameter RegistroAns = new ReportParameter("RegistroAns", atendimento.Convenio?.RegistroANS);
                    ReportParameter NumGuiaOperadora = new ReportParameter("NumGuiaOperadora", atendimento.GuiaNumero);
                    ReportParameter NumCarteira = new ReportParameter("NumCarteira", atendimento.Matricula);

                    ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", "");
                    if (atendimento.ValidadeCarteira.HasValue)
                    {
                        ValidadeCarteira = new ReportParameter("ValidadeCarteira", ((DateTime)atendimento.ValidadeCarteira).ToString("dd/MM/yyyy"));
                    }
                    ReportParameter NomeCompleto = new ReportParameter("NomeCompleto", atendimento.Paciente?.NomeCompleto);
                    ReportParameter NomeContratante = new ReportParameter("NomeContratante", atendimento.Empresa?.NomeFantasia);
                    ReportParameter CodCNES = new ReportParameter("CodCNES", atendimento.Empresa?.Cnes.ToString());
                    ReportParameter NomeMedicoExecutante = new ReportParameter("NomeMedicoExecutante", atendimento.Medico?.NomeCompleto);
                    ReportParameter ConselhoProfissional = new ReportParameter("ConselhoProfissional", dados.ConselhoProfissional);
                    ReportParameter NumeroConselho = new ReportParameter("NumeroConselho", atendimento.Medico?.NumeroConselho.ToString());
                    ReportParameter ConselhoUF = new ReportParameter("ConselhoUF", atendimento.Medico?.Conselho?.Uf);
                    ReportParameter CBO = new ReportParameter("CBO", atendimento.Especialidade?.Codigo);
                    ReportParameter DataAtendimento = new ReportParameter("DataAtendimento", atendimento.DataRegistro.ToString("dd/MM/yyyy"));
                    ReportParameter NumeroGuia = new ReportParameter("NumeroGuia", atendimento.GuiaNumero?.ToString());
                    ReportParameter RN = new ReportParameter("RN", dados.RN ? "S" : "N");
                    ReportParameter IndicacaoAcidente = new ReportParameter("IndicacaoAcidente", dados.IndicacaoAcidente);

                    viewer.LocalReport.SetParameters(new ReportParameter[] {
                            RegistroAns               ,
                            NumGuiaOperadora          ,
                            NumCarteira               ,
                            ValidadeCarteira          ,
                            NomeCompleto              ,
                            NomeContratante           ,
                            CodCNES                   ,
                            NomeMedicoExecutante      ,
                            ConselhoProfissional      ,
                            NumeroConselho            ,
                            ConselhoUF                ,
                            CBO                       ,
                            DataAtendimento           ,
                            NumeroGuia                ,
                            RN                        ,
                            IndicacaoAcidente

                });
                    // Fim - preenchendo parametros

                    // Pdf (byte array)
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";
                    byte[] pdfBytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    viewer.LocalReport.Refresh();
                    // Fim - Guia Principal

                    Response.Headers.Add("Content-Disposition", "inline; filename=guia_spsadt.pdf");
                    return File(pdfBytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;
        }

        public async Task<ActionResult> GuiaSpsadtPdf(long atendimentoId, long? contaId)
        {
            try
            {
                using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
                using (var contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
                using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
                {
                    var dados = new GuiaSpsadtModel();

                    List<FaturamentoContaItemDto> itensGuiaPrincipal = new List<FaturamentoContaItemDto>();
                    //    List<FaturamentoContaItemDto> itensGuiaComplementar = new List<FaturamentoContaItemDto>();
                    if (contaId.HasValue && contaId != 0)
                    {



                        var conta = await contaMedicaAppService.Object.ObterReportModel((long)contaId);
                        // Itens da conta
                        ListarFaturamentoContaItensInput listarItensInput = new ListarFaturamentoContaItensInput
                        {
                            Filtro = conta.Id.ToString()
                        };
                        var contaItensPaged = await contaItemAppService.Object.ListarPorConta(listarItensInput);
                        var contaItens = contaItensPaged.Items as List<FaturamentoContaItemDto>;
                        itensGuiaPrincipal = contaItens;
                        int totalItens = contaItens.Count;
                    }


                    var atendimento = await atendimentoAppService.Object.Obter(atendimentoId);
                    dados.LerAtendimento(atendimento);

                    // Guia principal
                    Guias relDS = new Guias();

                    // Logotipo
                    DataTable tabela = this.ConvertToDataTable(new List<string>() /*dados.Contas*/, relDS.Tables["Spsadt"]);
                    DataRow row = tabela.NewRow();
                    row["Logotipo"] = atendimento.Empresa.Logotipo;
                    tabela.Rows.Add(row);
                    // fim - logotipo

                    ReportDataSource dataSource = new ReportDataSource("Spsadt", tabela);
                    ReportViewer GuiaSpsadt = new ReportViewer();
                    GuiaSpsadt.LocalReport.DataSources.Add(dataSource);

                    ScriptManager scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(GuiaSpsadt);

                    GuiaSpsadt.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Spsadt\guia_spsadt.rdlc");
                    SetParametrosSpsadt(GuiaSpsadt, dados);

                    // Sub Relatorios, passando parametros
                    _itensGuiaPrincipal = itensGuiaPrincipal;
                    GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadt);
                    //    _itensGuiaPrincipalRealizados = itensGuiaPrincipalRealizados;
                    GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadtExames);
                    GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadtEquipe);

                    // Pdf (byte array)
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";
                    byte[] pdfBytes = GuiaSpsadt.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    GuiaSpsadt.LocalReport.Refresh();
                    // Fim - Guia Principal

                    // Guias complementares (Outras Despesas)
                    List<byte[]> pdfBytesComplementares = new List<byte[]>();

                    // Um relatorio extra para cada subLista de itens
                    //foreach (var lista in itensGuiaComplementar)
                    //{
                    //    Guias DS = new Guias();
                    //    List<GuiaSpsadtDespesaItem> despesaItens = new List<GuiaSpsadtDespesaItem>();

                    //    foreach (var item in lista)
                    //    {
                    //        var despesaItem = new GuiaSpsadtDespesaItem();
                    //        despesaItem.Descricao = item.Descricao + item.FaturamentoItem.Descricao;
                    //        despesaItem.Cd = "";
                    //        despesaItem.Qtde = item.Qtde.ToString();
                    //        despesaItens.Add(despesaItem);
                    //    }

                    //    DataTable tabelaOutrasDespesas = this.ConvertToDataTable(despesaItens, DS.Tables["SolicItens"]);
                    //    ReportDataSource dataSourceOutrasDepesas = new ReportDataSource("SolicItens", tabelaOutrasDespesas);
                    //    ReportViewer GuiaOutrasDespesas = new ReportViewer();
                    //    GuiaOutrasDespesas.LocalReport.DataSources.Add(dataSourceOutrasDepesas);
                    //    ScriptManager scriptManagerOutrasDespesas = new ScriptManager();
                    //    scriptManagerOutrasDespesas.RegisterPostBackControl(GuiaOutrasDespesas);
                    //    //GuiaOutrasDespesas.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\guia_spsadt_despesas.rdlc");
                    //    GuiaOutrasDespesas.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\InternacaoSolicitacao\guia_internacao_solic_itens.rdlc");

                    //    // PDF 'Outras Despesas'
                    //    Warning[] warningsOutras;
                    //    string[] streamIdsOutras;
                    //    string mimeTypeOutras = string.Empty;
                    //    string encodingOutras = string.Empty;
                    //    string extensionOutras = "pdf";
                    //    byte[] pdfBytesOutras = GuiaOutrasDespesas.LocalReport.Render("PDF", null, out mimeTypeOutras, out encodingOutras, out extensionOutras, out streamIdsOutras, out warningsOutras);
                    //    pdfBytesComplementares.Add(pdfBytesOutras);
                    //}
                    // FIM - Outras Despesas

                    // Anexando todos os relatorios
                    GuiaSpsadt.LocalReport.Refresh();
                    MemoryStream guiaPrincipalStream = new MemoryStream(pdfBytes); // guia principal
                    PdfDocument guiaPrincipalPdf = PdfReader.Open(guiaPrincipalStream, PdfDocumentOpenMode.Import);
                    PdfDocument pdfDefinitivo = new PdfDocument();

                    // Gerando paginas da guia principal
                    int principalPageCount = guiaPrincipalPdf.PageCount;
                    for (int i = 0; i < principalPageCount; i++)
                    {
                        PdfPage page = guiaPrincipalPdf.Pages[i];
                        page = pdfDefinitivo.AddPage(page);
                    }

                    // Guias complementares
                    foreach (var pdfBs in pdfBytesComplementares)
                    {
                        MemoryStream guiaComplementarStream = new MemoryStream(pdfBs); // guia outras despesas
                        PdfDocument guiaComplementarPdf = PdfReader.Open(guiaComplementarStream, PdfDocumentOpenMode.Import);
                        int compPageCount = guiaComplementarPdf.PageCount;
                        for (int i = 0; i < compPageCount; i++)
                        {
                            PdfPage page = guiaComplementarPdf.Pages[i];
                            page = pdfDefinitivo.AddPage(page);
                        }
                    }

                    // Gerando pdf byte array, relatorio definitivo
                    byte[] definitivaBytes = null;

                    using (MemoryStream stream = new MemoryStream())
                    {
                        pdfDefinitivo.Save(stream, true);
                        definitivaBytes = stream.ToArray();
                    }

                    Response.Headers.Add("Content-Disposition", "inline; filename=guia_spsadt.pdf");
                    return File(definitivaBytes, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return null;
        }

        // FIM - Novo modelo

        public ActionResult Index()
        {
            var model = new ContasMedicasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/Index.cshtml");
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var fatConta = new ClassesAplicacao.Services.Faturamentos.Contas.Dto.FaturamentoContaDto();
            var model = new RelSolicInternacaoModalViewModel(fatConta);

            if (id.HasValue)
            {
                //  model = await _guiaAppService.Obter((long)id);
            }
            else
            {
                model.Id = 0;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/Consulta/_CriarOuEditarModal.cshtml");
        }

        public async Task<ActionResult> VisualizarSpsadt(GuiaSpsadtInput input)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            {
                var dados = new GuiaSpsadtModel();
                var conta = await contaMedicaAppService.Object.ObterReportModel((long)input.ContaId);
                var atendimento = await atendimentoAppService.Object.Obter((long)input.AtendimentoId);
                dados.LerAtendimento(atendimento);

                // Itens da conta
                ListarFaturamentoContaItensInput listarItensInput = new ListarFaturamentoContaItensInput
                {
                    Filtro = conta.Id.ToString()
                };
                var contaItensPaged = await contaItemAppService.Object.ListarPorConta(listarItensInput);
                var contaItens = contaItensPaged.Items as List<FaturamentoContaItemDto>;
                int totalItens = contaItens.Count;

                //   bool gerarGuiaComplementar = totalItens > 5 ? true : false;

                //    List<FaturamentoContaItemDto> itensGuiaPrincipal = new List<FaturamentoContaItemDto>();
                //     List<FaturamentoContaItemDto> itensGuiaPrincipalRealizados = new List<FaturamentoContaItemDto>();
                //    List<List<FaturamentoContaItemDto>> itensGuiaComplementar = new List<List<FaturamentoContaItemDto>>();

                // Verificando necessidade de guias complementares, gerando subListas de itens se necessario
                //if (gerarGuiaComplementar)
                //{
                //    itensGuiaPrincipal = contaItens.GetRange(0, 5);
                //    var lista = contaItens.GetRange(5, totalItens - 5);
                //    // Cada guia complementar suporta ate 7 itens
                //    var tamanho = 7;

                //    // Gerando subListas de ate 7 itens
                //    for (int i = 0; i < lista.Count; i += tamanho)
                //    {
                //        itensGuiaComplementar.Add(lista.GetRange(i, Math.Min(tamanho, lista.Count - i)));
                //    }

                //    // itensGuiaComplementar = repartirLista(contaItens.GetRange(4, totalItens - 5), 7) as List<List<FaturamentoContaItemDto>>;
                //}
                //     else
                //     {
                // itensGuiaPrincipal = contaItens.GetRange(0, totalItens);

                // Complementar com itens vazios para ocupar espaco certo no relatorio
                //int count = itensGuiaPrincipal.Count;

                //if (count < 5)
                //{
                //    int diferenca = 5 - count;

                //    for (int i = diferenca + 1; i < 5; i++)
                //    {
                //        var novoItem = new FaturamentoContaItemDto();
                //        novoItem.FaturamentoItem = new FaturamentoItemDto();
                //        itensGuiaPrincipal.Add(novoItem);
                //    }
                //}
                //  }

                try
                {
                    // Guia principal
                    Guias relDS = new Guias();

                    DataTable tabela = this.ConvertToDataTable(dados.Contas, relDS.Tables["SolicItens"]);//precisa?
                    ReportDataSource dataSource = new ReportDataSource("SolicItens", tabela);
                    ReportViewer GuiaSpsadt = new ReportViewer();
                    GuiaSpsadt.LocalReport.DataSources.Add(dataSource);

                    ScriptManager scriptManager = new ScriptManager();
                    scriptManager.RegisterPostBackControl(GuiaSpsadt);
                    GuiaSpsadt.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\Consulta\guia_consulta.rdlc");
                    //SetParametrosSpsadt(GuiaSpsadt, dados);

                    //// Sub Relatorios, passando parametros
                    //_itensGuiaPrincipal = itensGuiaPrincipal;
                    //GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadt);
                    ////    _itensGuiaPrincipalRealizados = itensGuiaPrincipalRealizados;
                    //GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadtExames);
                    //GuiaSpsadt.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(LocalReport_SubreportProcessingSpsadtEquipe);

                    // Pdf (byte array)
                    Warning[] warnings;
                    string[] streamIds;
                    string mimeType = string.Empty;
                    string encoding = string.Empty;
                    string extension = "pdf";
                    byte[] pdfBytes = GuiaSpsadt.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);
                    GuiaSpsadt.LocalReport.Refresh();
                    // Fim - Guia Principal

                    //// Guias complementares (Outras Despesas)
                    //List<byte[]> pdfBytesComplementares = new List<byte[]>();

                    //// Um relatorio extra para cada subLista de itens
                    //foreach (var lista in itensGuiaComplementar)
                    //{
                    //    Guias DS = new Guias();
                    //    List<GuiaSpsadtDespesaItem> despesaItens = new List<GuiaSpsadtDespesaItem>();

                    //    foreach (var item in lista)
                    //    {
                    //        var despesaItem = new GuiaSpsadtDespesaItem();
                    //        despesaItem.Descricao = item.Descricao + item.FaturamentoItem.Descricao;
                    //        despesaItem.Cd = "";
                    //        despesaItem.Qtde = item.Qtde.ToString();
                    //        despesaItens.Add(despesaItem);
                    //    }

                    //    DataTable tabelaOutrasDespesas = this.ConvertToDataTable(despesaItens, DS.Tables["SolicItens"]);
                    //    ReportDataSource dataSourceOutrasDepesas = new ReportDataSource("SolicItens", tabelaOutrasDespesas);
                    //    ReportViewer GuiaOutrasDespesas = new ReportViewer();
                    //    GuiaOutrasDespesas.LocalReport.DataSources.Add(dataSourceOutrasDepesas);
                    //    ScriptManager scriptManagerOutrasDespesas = new ScriptManager();
                    //    scriptManagerOutrasDespesas.RegisterPostBackControl(GuiaOutrasDespesas);
                    //    //GuiaOutrasDespesas.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\guia_spsadt_despesas.rdlc");
                    //    GuiaOutrasDespesas.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\InternacaoSolicitacao\guia_internacao_solic_itens.rdlc");

                    //    // PDF 'Outras Despesas'
                    //    Warning[] warningsOutras;
                    //    string[] streamIdsOutras;
                    //    string mimeTypeOutras = string.Empty;
                    //    string encodingOutras = string.Empty;
                    //    string extensionOutras = "pdf";
                    //    byte[] pdfBytesOutras = GuiaOutrasDespesas.LocalReport.Render("PDF", null, out mimeTypeOutras, out encodingOutras, out extensionOutras, out streamIdsOutras, out warningsOutras);
                    //    pdfBytesComplementares.Add(pdfBytesOutras);
                    //}
                    //// FIM - Outras Despesas

                    // Anexando todos os relatorios
                    //GuiaSpsadt.LocalReport.Refresh();
                    //MemoryStream guiaPrincipalStream = new MemoryStream(pdfBytes); // guia principal
                    //PdfDocument guiaPrincipalPdf = PdfReader.Open(guiaPrincipalStream, PdfDocumentOpenMode.Import);
                    //PdfDocument pdfDefinitivo = new PdfDocument();

                    // Gerando paginas da guia principal
                    //int principalPageCount = guiaPrincipalPdf.PageCount;
                    //for (int i = 0; i < principalPageCount; i++)
                    //{
                    //    PdfPage page = guiaPrincipalPdf.Pages[i];
                    //    page = pdfDefinitivo.AddPage(page);
                    //}

                    //// Guias complementares
                    //foreach (var pdfBs in pdfBytesComplementares)
                    //{
                    //    MemoryStream guiaComplementarStream = new MemoryStream(pdfBs); // guia outras despesas
                    //    PdfDocument guiaComplementarPdf = PdfReader.Open(guiaComplementarStream, PdfDocumentOpenMode.Import);
                    //    int compPageCount = guiaComplementarPdf.PageCount;
                    //    for (int i = 0; i < compPageCount; i++)
                    //    {
                    //        PdfPage page = guiaComplementarPdf.Pages[i];
                    //        page = pdfDefinitivo.AddPage(page);
                    //    }
                    //}

                    // Gerando pdf byte array, relatorio definitivo
                    // byte[] definitivaBytes = null;

                    //using (MemoryStream stream = new MemoryStream())
                    //{
                    //    pdfDefinitivo.Save(stream, true);
                    //    definitivaBytes = stream.ToArray();
                    //}

                    Response.Headers.Add("Content-Disposition", "inline; filename=guia_spsadt.pdf");
                    return File(pdfBytes, "application/pdf");
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }

                return null;
            }
        }

        public void SetParametrosSpsadt(ReportViewer rv, GuiaSpsadtModel dados)
        {
            ReportParameter NumeroGuiaPrestador = new ReportParameter("NumeroGuiaPrestador", dados.NumeroGuiaPrestador);
            ReportParameter RegistroAns = new ReportParameter("RegistroAns", dados.RegistroAns);
            ReportParameter NumeroCarteira = new ReportParameter("NumeroCarteira", dados.NumeroCarteira);
            ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", dados.ValidadeCarteira);
            ReportParameter NomePaciente = new ReportParameter("NomePaciente", string.IsNullOrEmpty(dados.NomePaciente) ? " " : dados.NomePaciente);
            ReportParameter NomeContratado = new ReportParameter("NomeContratado", string.IsNullOrEmpty(dados.NomeContratado) ? " " : dados.NomeContratado);
            ReportParameter NomeProfissionalSolicitante = new ReportParameter("NomeProfissionalSolicitante", dados.NomeProfissionalSolicitante);
            ReportParameter ConselhoProfissional = new ReportParameter("ConselhoProfissional", dados.ConselhoProfissional);
            ReportParameter NumeroConselho = new ReportParameter("NumeroConselho", dados.NumeroConselho);
            ReportParameter UF = new ReportParameter("UF", dados.UF);
            ReportParameter CodigoCbo = new ReportParameter("CodigoCbo", dados.CodigoCbo);
            ReportParameter IndicacaoClinica = new ReportParameter("IndicacaoClinica", dados.IndicacaoClinica);
            ReportParameter Senha = new ReportParameter("Senha", string.IsNullOrEmpty(dados.Senha) ? " " : dados.Senha);
            ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", dados.DataAutorizacao);
            ReportParameter DataValidadeSenha = new ReportParameter("DataValidadeSenha", dados.DataValidadeSenha);
            ReportParameter RN = new ReportParameter("RN", dados.RN ? "S" : "N");
            ReportParameter NumeroGuiaOperadora = new ReportParameter("NumeroGuiaOperadora", string.IsNullOrEmpty(dados.NumeroGuiaOperadora) ? " " : dados.NumeroGuiaOperadora);
            ReportParameter DataSolicitacao = new ReportParameter("DataSolicitacao", dados.DataSolicitacao);
            ReportParameter CodigoCnes = new ReportParameter("CodigoCnes", dados.CodigoCne);
            ReportParameter TipoAtendimento = new ReportParameter("TipoAtendimento", dados.TipoAtendimento);
            ReportParameter TotalProcedimentos = new ReportParameter("TotalProcedimentos", dados.TotalProcedimentos);
            ReportParameter TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", dados.TotalTaxasAlugueis);
            ReportParameter TotalMateriais = new ReportParameter("TotalMateriais", dados.TotalMateriais);
            ReportParameter TotalOpme = new ReportParameter("TotalOpme", dados.TotalOpme);
            ReportParameter TotalMedicamentos = new ReportParameter("TotalMedicamentos", dados.TotalMedicamentos);
            ReportParameter TotalGasesMedicinais = new ReportParameter("TotalGasesMedicinais", dados.TotalGasesMedicinais);
            ReportParameter TotalGeral = new ReportParameter("TotalGeral", dados.TotalGeral);


            rv.LocalReport.SetParameters(new ReportParameter[] {
                            NumeroGuiaPrestador            ,
                            RegistroAns                    ,
                            DataAutorizacao                ,
                            Senha                          ,
                            NumeroGuiaOperadora            ,
                            DataValidadeSenha              ,
                            NumeroCarteira                 ,
                            ValidadeCarteira               ,
                            NomePaciente                   ,
                            DataSolicitacao                ,
                            NomeContratado                 ,
                            NomeProfissionalSolicitante    ,
                            ConselhoProfissional           ,
                            NumeroConselho                 ,
                            UF                             ,
                            CodigoCbo                      ,
                            RN                             ,
                            IndicacaoClinica               ,
                            CodigoCnes                     ,
                            TipoAtendimento                ,
                            TotalProcedimentos             ,
                            TotalTaxasAlugueis             ,
                            TotalMateriais                 ,
                            TotalOpme                      ,
                            TotalMedicamentos              ,
                            TotalGasesMedicinais           ,
                            TotalGeral                     ,


          });
        }

        public void LocalReport_SubreportProcessingSpsadt(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "12313212";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Itens solicitados
                List<GuiaSpsadtItemSolic> itensSolics = new List<GuiaSpsadtItemSolic>();

                foreach (var item in _itensGuiaPrincipal)
                {
                    var ex1 = new GuiaSpsadtItemSolic();
                    ex1.CodigoProcedimento = "";
                    ex1.Descricao = item.Descricao + " " + item.FaturamentoItem.Descricao;
                    ex1.QtAutoriz = "";
                    ex1.QtSolic = item.Qtde > 0 ? item.Qtde.ToString() : "";
                    ex1.Tabela = "";

                    itensSolics.Add(ex1);
                }

                DataTable tabelaItensSolic = this.ConvertToDataTable(itensSolics, relDS.Tables["SpsadtItensSolic"]);
                ReportDataSource dataSourceItensSolic = new ReportDataSource("GuiaSpsadtItensSolic", tabelaItensSolic);
                e.DataSources.Add(dataSourceItensSolic);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void LocalReport_SubreportProcessingSpsadtExames(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "1234";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Exames
                List<GuiaSpsadtExame> exames = new List<GuiaSpsadtExame>();

                // Itens solicitados
                List<GuiaSpsadtItemSolic> itensSolics = new List<GuiaSpsadtItemSolic>();

                foreach (var item in _itensGuiaPrincipal)
                {
                    var ex1 = new GuiaSpsadtExame();
                    ex1.CodigoProcedimento = "";
                    ex1.Descricao = item.Descricao + " " + item.FaturamentoItem.Descricao;
                    ex1.Tabela = "";
                    ex1.Data = item.Data?.ToString("dd/MM/yy");
                    ex1.HoraInicial = item.HoraIncio?.ToString("mm:ss");
                    ex1.HoraFinal = item.HoraFim?.ToString("mm:ss tt");
                    ex1.Qtde = item.Qtde > 0 ? item.Qtde.ToString() : "";
                    ex1.ValorUnitario = item.ValorItem > 0 ? item.ValorItem.ToString() : "";
                    ex1.ValorTotal = (item.ValorItem * item.Qtde) > 0 ? (item.ValorItem * item.Qtde).ToString() : "";
                    exames.Add(ex1);
                }

                DataTable tabelaExames = this.ConvertToDataTable(exames, relDS.Tables["SpsadtExames"]);
                ReportDataSource dataSourceExames = new ReportDataSource("GuiaSpsadtExames", tabelaExames);
                e.DataSources.Add(dataSourceExames);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public void LocalReport_SubreportProcessingSpsadtEquipe(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                var dados = new GuiaSpsadtModel();
                dados.Contas = new List<ContaMedicaReportModel>();
                var x = new ContaMedicaReportModel();
                x.AtendimentoCodigo = "123";
                dados.Contas.Add(x);
                Guias relDS = new Guias();

                // Equipe

                List<GuiaSpsadtEquipe> equipe = new List<GuiaSpsadtEquipe>();
                for (int i = 0; i < 4; i++)
                {
                    var ex1 = new GuiaSpsadtEquipe();
                    ex1.CodigoCbo = "";
                    ex1.CodigoOperadoraCpf = "";
                    ex1.ConselhoProfissional = "";
                    ex1.GrauPart = "";
                    ex1.NomeProfissional = "";
                    ex1.NumeroConselho = "";
                    ex1.SeqRef = "";
                    ex1.Uf = "";

                    equipe.Add(ex1);
                }

                DataTable tabelaEquipe = this.ConvertToDataTable(equipe, relDS.Tables["SpsadtEquipe"]);
                ReportDataSource dataSourceEquipe = new ReportDataSource("GuiaSpsadtEquipe", tabelaEquipe);
                e.DataSources.Add(dataSourceEquipe);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //public void LocalReport_SubreportProcessingSpsadtDespesas(object sender, SubreportProcessingEventArgs e)
        //{
        //    try
        //    {
        //        var dados = new GuiaSpsadtModel();
        //        dados.Contas = new List<ContaMedicaReportModel>();
        //        var x = new ContaMedicaReportModel();
        //        x.AtendimentoCodigo = "12313212";
        //        dados.Contas.Add(x);
        //        Guias relDS = new Guias();

        //        // Despesa Itens

        //        List<GuiaSpsadtDespesaItem> equipe = new List<GuiaSpsadtDespesaItem>();
        //        for (int i = 0; i < 7; i++)
        //        {
        //            var ex1 = new GuiaSpsadtDespesaItem();
        //            ex1.Descricao = "" + i.ToString();
        //            ex1.Cd = "" + i.ToString();
        //            equipe.Add(ex1);
        //        }

        //        DataTable tabelaEquipe = this.ConvertToDataTable(equipe, relDS.Tables["SpsadtDespesasItens"]);
        //        ReportDataSource dataSourceEquipe = new ReportDataSource("SpsadtDespesasItens", tabelaEquipe);
        //        e.DataSources.Add(dataSourceEquipe);

        //        // Despesa loop

        //        //List<teste> loop = new List<teste>();
        //        //for (int i = 0; i < 4; i++)
        //        //{
        //        //    var ex1 = new teste();
        //        //    ex1.Despesas = "despesas" + i.ToString();

        //        //    loop.Add(ex1);
        //        //}

        //        //DataTable tabelaEquipeloop = this.ConvertToDataTable(loop, relDS.Tables["SpsadtDespesas"]);
        //        //ReportDataSource dataSourceEquipeloop = new ReportDataSource("SpsadtDespesasLoopDataSet", tabelaEquipeloop);
        //        //e.DataSources.Add(dataSourceEquipeloop);
        //    }
        //    catch (Exception ex)
        //    {
        //        ex.ToString();
        //    }
        //}

        public DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
        {
            PropertyDescriptorCollection properties =
               TypeDescriptor.GetProperties(typeof(T));

            if (data != null)
            {
                foreach (T item in data)
                {
                    try
                    {
                        DataRow row = table.NewRow();
                        foreach (PropertyDescriptor prop in properties)
                            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        table.Rows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        ex.ToString();
                    }
                }
            }

            return table;
        }


    }

    //public class GuiaSpsadtItemSolic
    //{
    //    public string Tabela { get; set; }
    //    public string CodigoProcedimento { get; set; }
    //    public string Descricao { get; set; }
    //    public string QtSolic { get; set; }
    //    public string QtAutoriz { get; set; }
    //}

    //public class GuiaSpsadtExame
    //{
    //    public string Data { get; set; }
    //    public string HoraInicial { get; set; }
    //    public string HoraFinal { get; set; }
    //    public string Tabela { get; set; }
    //    public string CodigoProcedimento { get; set; }
    //    public string Descricao { get; set; }
    //    public string Qtde { get; set; }
    //    public string Via { get; set; }
    //    public string Tec { get; set; }
    //    public string RedAcresc { get; set; }
    //    public string ValorUnitario { get; set; }
    //    public string ValorTotal { get; set; }
    //}

    //public class GuiaSpsadtEquipe
    //{
    //    public string SeqRef { get; set; }
    //    public string GrauPart { get; set; }
    //    public string CodigoOperadoraCpf { get; set; }
    //    public string NomeProfissional { get; set; }
    //    public string ConselhoProfissional { get; set; }
    //    public string NumeroConselho { get; set; }
    //    public string Uf { get; set; }
    //    public string CodigoCbo { get; set; }
    //}

    //public class GuiaSpsadtDespesaItem
    //{
    //    public string Cd { get; set; }
    //    public string Data { get; set; }
    //    public string HoraInicial { get; set; }
    //    public string HoraFinal { get; set; }
    //    public string Tabela { get; set; }
    //    public string CodigoItem { get; set; }
    //    public string Qtde { get; set; }
    //    public string UnidadeMedida { get; set; }
    //    public string RedAcres { get; set; }
    //    public string ValorUnitario { get; set; }
    //    public string ValorTotal { get; set; }
    //    public string RegistroAnvisa { get; set; }
    //    public string RefMaterialFabricante { get; set; }
    //    public string NumAutorizacaoFuncionamento { get; set; }
    //    public string Descricao { get; set; }

    //    public void LerContaItem(FaturamentoContaItem item)
    //    {
    //        Cd = "";
    //        Data = item.Data?.ToString("dd/MM/yyyy");
    //        HoraInicial = item.HoraIncio.ToString();
    //        HoraFinal = item.HoraFim.ToString();
    //        Tabela = "";
    //        CodigoItem = item.Codigo;
    //        Qtde = item.Qtde.ToString();
    //        UnidadeMedida = "";
    //        RedAcres = "";
    //        ValorUnitario = "";
    //        ValorTotal = "";
    //        RegistroAnvisa = "";
    //        RefMaterialFabricante = "";
    //        NumAutorizacaoFuncionamento = "";
    //        Descricao = item.Descricao;
    //    }
    //}

}