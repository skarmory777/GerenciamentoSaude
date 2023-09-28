#region Usings
using Abp.Dependency;
using Microsoft.Reporting.WebForms;
using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItens.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ContaItenss;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Guias;
using SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Faturamentos;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ContasMedicas;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios.Guias;
using SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.ResumuInternacao;
using SW10.SWMANAGER.Web.Controllers;
using SW10.SWMANAGER.Web.Relatorios.Faturamento.Guias;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
#endregion usings.

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.GuiaResumoInternacao
{
    public class FatGuiasResumoInternacaoController : SWMANAGERControllerBase
    {

        // Dados Guia Spsadt
        List<FaturamentoContaItemDto> _itensGuiaPrincipal = new List<FaturamentoContaItemDto>();

        public ActionResult Index()
        {
            var model = new ContasMedicasViewModel();
            return View("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ContasMedicas/Index.cshtml");
        }

        public async Task<ActionResult> CriarOuEditarModal(long? id)
        {
            var fatConta = new ClassesAplicacao.Services.Faturamentos.Contas.Dto.FaturamentoContaDto();
            var model = new RelResumoInternacaoModalViewModel(fatConta);

            if (id.HasValue)
            {
                //  model = await _guiaAppService.Obter((long)id);
            }
            else
            {
                model.Id = 0;
            }
            return PartialView("~/Areas/Mpa/Views/Aplicacao/Faturamentos/ResumoInternacao/_CriarOuEditarModal.cshtml");
        }

        public async Task<ActionResult> VisualizarSpsadt(GuiaSpsadtInput input)
        {
            using (var contaMedicaAppService = IocManager.Instance.ResolveAsDisposable<IContaAppService>())
            using (var atendimentoAppService = IocManager.Instance.ResolveAsDisposable<IAtendimentoAppService>())
            using (var contaItemAppService = IocManager.Instance.ResolveAsDisposable<IFaturamentoContaItemAppService>())
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
                    GuiaSpsadt.LocalReport.ReportPath = string.Concat(Server.MapPath("~"), @"Relatorios\Faturamento\Guias\InternacaoResumo\guia_internacao_resumo.rdlc");
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


            ReportParameter NumeroGuiaPrestador = new ReportParameter("NumeroGuiaPrestador", "000002424882");
            ReportParameter RegistroAns = new ReportParameter("RegistroAns", "326305");
            //ReportParameter NumeroGuiaOperadora = new ReportParameter("NumeroGuiaOperadora", string.IsNullOrEmpty(dados.NumeroGuiaOperadora) ? " " : dados.NumeroGuiaOperadora);
            // ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", "25/01/2018");
            // ReportParameter Senha = new ReportParameter("Senha", string.IsNullOrEmpty(dados.Senha) ? " " : dados.Senha);
            // ReportParameter DataValidadeSenha = new ReportParameter("DataValidadeSenha", "24/02/2018");
            ReportParameter NumeroCarteira = new ReportParameter("NumeroCarteira", "315948132");
            ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", "25 01 2018");
            ReportParameter NomePaciente = new ReportParameter("NomePaciente", string.IsNullOrEmpty(dados.NomePaciente) ? " " : dados.NomePaciente);
            ReportParameter AtendimentoRn = new ReportParameter("AtendimentoRn", string.IsNullOrEmpty(dados.AtendimentoRn) ? " " : dados.AtendimentoRn);

            //ReportParameter CartaoNacionalSaude = new ReportParameter("CartaoNacionalSaude", string.IsNullOrEmpty(dados.NumeroCns) ? " " : dados.NumeroCns);
            //ReportParameter CodigoOperadora = new ReportParameter("CodigoOperadora", string.IsNullOrEmpty(dados.CodigoOperadora) ? " " : dados.CodigoOperadora);
            ReportParameter NomeContratado = new ReportParameter("NomeContratado", string.IsNullOrEmpty(dados.NomeContratado) ? " " : dados.NomeContratado);
            ReportParameter NomeProfissionalSolicitante = new ReportParameter("NomeProfissionalSolicitante", "Guilerme Morais Nunes");
            ReportParameter ConselhoProfissional = new ReportParameter("ConselhoProfissional", "6");
            ReportParameter NumeroConselho = new ReportParameter("NumeroConselho", "52.103162-7");
            ReportParameter UF = new ReportParameter("UF", "33");
            ReportParameter CodigoCbo = new ReportParameter("CodigoCbo", "2251.25");
            //ReportParameter CaraterAtendimento = new ReportParameter("CaraterAtendimento", string.IsNullOrEmpty(dados.CaraterAtendimento) ? " " : dados.CaraterAtendimento);
            //ReportParameter TipoAtendimento = new ReportParameter("TipoAtendimento", string.IsNullOrEmpty(dados.TipoAtendimento) ? " " : dados.TipoAtendimento);
            ReportParameter IndicacaoClinica = new ReportParameter("IndicacaoClinica", "IndicacaoClinica");
            //ReportParameter IndicacaoAcidente = new ReportParameter("IndicacaoAcidente", string.IsNullOrEmpty(dados.IndicacaoAcidente) ? " " : dados.IndicacaoAcidente);
            ////=====acertar fonte de dados===========
            ReportParameter RegimeInternacao = new ReportParameter("RegimeInternacao", "1");
            ReportParameter CodigoOperadoraCnpj = new ReportParameter("CodigoOperadoraCnpj", "10429476");
            ReportParameter NomeHospital = new ReportParameter("NomeHospital", "American Cor");
            ReportParameter DataSugerInterna = new ReportParameter("DataSugerInterna", string.IsNullOrEmpty(dados.DataSolicitacao) ? " " : dados.DataSolicitacao);
            ReportParameter QtdDiariasSolicitadas = new ReportParameter("QtdDiariasSolicitadas", "25");
            ReportParameter PrevOPME = new ReportParameter("PrevOPME", "Prev OPME");
            ReportParameter PrevQuimio = new ReportParameter("PrevQuimio", "Prev");
            ReportParameter Cid1 = new ReportParameter("Cid1", "Cid1");
            ReportParameter Cid2 = new ReportParameter("Cid2", "Cid2");
            ReportParameter Cid3 = new ReportParameter("Cid3", "Cid3");
            ReportParameter Cid4 = new ReportParameter("Cid4", "Cid4");
            ///////////////////////////////////////////////////////////////////

            rv.LocalReport.SetParameters(new ReportParameter[] {
                            NumeroGuiaPrestador ,
                            RegistroAns ,
                             //DataAutorizacao,
                             //Senha ,
                             //NumeroGuiaOperadora ,
                             //DataValidadeSenha,
                            NumeroCarteira ,
                            ValidadeCarteira ,
                            NomePaciente ,
                            //CartaoNacionalSaude ,
                            AtendimentoRn ,
                             //CodigoOperadora ,
                            NomeContratado ,
                             NomeProfissionalSolicitante ,
                             ConselhoProfissional ,
                             NumeroConselho ,
                             UF ,
                             CodigoCbo ,
                             //AtendimentoRn,
                             //CaraterAtendimento ,
                             IndicacaoClinica ,
                             //TipoAtendimento ,
                            //IndicacaoAcidente ,
                            RegimeInternacao ,
                            CodigoOperadoraCnpj,
                            NomeHospital ,
                            DataSugerInterna ,
                            QtdDiariasSolicitadas,
                            PrevOPME,
                            PrevQuimio,
                            Cid1,
                            Cid2,
                            Cid3,
                            Cid4
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