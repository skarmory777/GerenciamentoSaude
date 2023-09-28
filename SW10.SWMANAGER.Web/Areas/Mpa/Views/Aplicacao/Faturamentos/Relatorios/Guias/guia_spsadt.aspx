<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.ComponentModel" %>
<%@ Import Namespace="SW10.SWMANAGER.Web.Relatorios.Faturamento.Guias" %>
<!-- namespace para localizar FiltroModel-->
<%@ Import Namespace="SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios.Guias" %>
<!-- FIM - namespace para localizar FiltroModel-->

<%@ Page Language="C#" EnableEventValidation="false" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrescricaoMedica.aspx.cs" Inherits="SW10.SWMANAGER.Web.Areas.Mpa.Views.Aplicacao.Assistenciais.AmbulatoriosEmergencias.Relatorios.PrescricoesMedicas.PrescricaoMedica" %>--%>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta name="viewport" content="width=device-width" />
    <meta http-equiv="x-ua-compatible" content="IE=9">
    <title>Visualizar</title>
    <script src="/Common/Scripts/ReportsJSFix.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div>
            <script runat="server">
                private void Page_Load (object sender, System.EventArgs e)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(this.GuiaSpsadt);


                    //localização do relatório
                    var dados = (GuiaSpsadtModel)Model;
                    string path = HttpContext.Current.Server.MapPath("~");
                    GuiaSpsadt.LocalReport.ReportPath = string.Concat(path, @"Relatorios\Faturamento\Guias\Spsadt\guia_spsadt.rdlc");
                    GuiaSpsadt.LocalReport.EnableExternalImages = true;

                    //var y = new ReportParameter()

                    if (dados != null)
                    {
                        ReportParameter NumeroGuiaPrestador = new ReportParameter("NumeroGuiaPrestador",string.IsNullOrEmpty(dados.NumeroGuiaPrestador) ? "NumGuiaPrest" : dados.NumeroGuiaPrestador );
                        ReportParameter RegistroAns = new ReportParameter("RegistroAns", string.IsNullOrEmpty(dados.RegistroAns) ? "registro ans" : dados.RegistroAns );
                        ReportParameter NumeroGuiaPrincipal = new ReportParameter("NumeroGuiaPrincipal", string.IsNullOrEmpty(dados.NumeroGuiaPrincipal) ? "num guia principal" : dados.NumeroGuiaPrincipal );
                        ReportParameter DataAutorizacao = new ReportParameter("DataAutorizacao", string.IsNullOrEmpty(dados.DataAutorizacao) ? "data aut" : dados.DataAutorizacao );
                        ReportParameter Senha = new ReportParameter("Senha", string.IsNullOrEmpty(dados.Senha) ? "12345687" : dados.Senha );
                        ReportParameter DataValidadeSenha = new ReportParameter("DataValidadeSenha", string.IsNullOrEmpty(dados.DataValidadeSenha) ? "data senha" : dados.DataValidadeSenha );
                        ReportParameter NumeroGuiaOperadora = new ReportParameter("NumeroGuiaOperadora", string.IsNullOrEmpty(dados.NumeroGuiaOperadora) ? "num guia oper" : dados.NumeroGuiaOperadora );
                        ReportParameter NumeroCarteira = new ReportParameter("NumeroCarteira", string.IsNullOrEmpty(dados.NumeroCarteira) ? "num carte" : dados.NumeroCarteira );
                        ReportParameter ValidadeCarteira = new ReportParameter("ValidadeCarteira", string.IsNullOrEmpty(dados.ValidadeCarteira) ? "valid cart" : dados.ValidadeCarteira );
                        ReportParameter NomePaciente = new ReportParameter("NomePaciente", string.IsNullOrEmpty(dados.NomePaciente) ? "nome pac" : dados.NomePaciente );
                        ReportParameter NumeroCns = new ReportParameter("NumeroCns", string.IsNullOrEmpty(dados.NumeroCns) ? "num cns" : dados.NumeroCns );
                        ReportParameter AtendimentoRn = new ReportParameter("AtendimentoRn", string.IsNullOrEmpty(dados.AtendimentoRn) ? "atend rn" : dados.AtendimentoRn );
                        ReportParameter CodigoOperadora = new ReportParameter("CodigoOperadora", string.IsNullOrEmpty(dados.CodigoOperadora) ? "cod oper" : dados.CodigoOperadora );
                        ReportParameter NomeContratado = new ReportParameter("NomeContratado", string.IsNullOrEmpty(dados.NomeContratado) ? "nome contrat" : dados.NomeContratado );
                        ReportParameter NomeProfissionalSolicitante = new ReportParameter("NomeProfissionalSolicitante", string.IsNullOrEmpty(dados.NomeProfissionalSolicitante) ? "nome prof solic" : dados.NomeProfissionalSolicitante );
                        ReportParameter ConselhoProfissional = new ReportParameter("ConselhoProfissional", string.IsNullOrEmpty(dados.ConselhoProfissional) ? "cons prof" : dados.ConselhoProfissional );
                        ReportParameter NumeroConselho = new ReportParameter("NumeroConselho", string.IsNullOrEmpty(dados.NumeroConselho) ? "num conselho" : dados.NumeroConselho );
                        ReportParameter UF = new ReportParameter("UF", string.IsNullOrEmpty(dados.UF) ? "uf" : dados.UF );
                        ReportParameter CodigoCbo = new ReportParameter("CodigoCbo", string.IsNullOrEmpty(dados.CodigoCbo) ? "cod cbo" : dados.CodigoCbo );
                        ReportParameter AssinaturaProfissionalSolicitante = new ReportParameter("AssinaturaProfissionalSolicitante", string.IsNullOrEmpty(dados.AssinaturaProfissionalSolicitante) ? "assin prof" : dados.AssinaturaProfissionalSolicitante );
                        ReportParameter CaraterAtendimento = new ReportParameter("CaraterAtendimento", string.IsNullOrEmpty(dados.CaraterAtendimento) ? "carater atend" : dados.CaraterAtendimento );
                        ReportParameter DataSolicitacao = new ReportParameter("DataSolicitacao", string.IsNullOrEmpty(dados.DataSolicitacao) ? "data solic" : dados.DataSolicitacao );
                        ReportParameter IndicacaoClinica = new ReportParameter("IndicacaoClinica", string.IsNullOrEmpty(dados.IndicacaoClinica) ? "indic clinica" : dados.IndicacaoClinica );
                        ReportParameter CodigoCne = new ReportParameter("CodigoCne", string.IsNullOrEmpty(dados.CodigoCne) ? "cod cne" : dados.CodigoCne );
                        ReportParameter TipoAtendimento = new ReportParameter("TipoAtendimento", string.IsNullOrEmpty(dados.TipoAtendimento) ? "tipo atend" : dados.TipoAtendimento );
                        ReportParameter IndicacaoAcidente = new ReportParameter("IndicacaoAcidente",string.IsNullOrEmpty(dados.IndicacaoAcidente) ? "indic acid" : dados.IndicacaoAcidente );
                        ReportParameter TipoConsulta = new ReportParameter("TipoConsulta", string.IsNullOrEmpty(dados.TipoConsulta) ? "tipo consulta" : dados.TipoConsulta );
                        ReportParameter MotivoEncerramentoAtendimento = new ReportParameter("MotivoEncerramentoAtendimento", string.IsNullOrEmpty(dados.MotivoEncerramentoAtendimento) ? "motivo encer" : dados.MotivoEncerramentoAtendimento );
                        // Identificacao Equipe
                        ReportParameter SequenciaRef1 = new ReportParameter("SequenciaRef1", string.IsNullOrEmpty(dados.SequenciaRef1) ? "12345687" : dados.SequenciaRef1 );
                        ReportParameter SequenciaRef2 = new ReportParameter("SequenciaRef2", string.IsNullOrEmpty(dados.SequenciaRef2) ? "12345687" : dados.SequenciaRef2 );
                        ReportParameter SequenciaRef3 = new ReportParameter("SequenciaRef3", string.IsNullOrEmpty(dados.SequenciaRef3) ? "12345687" : dados.SequenciaRef3 );
                        ReportParameter SequenciaRef4 = new ReportParameter("SequenciaRef4", string.IsNullOrEmpty(dados.SequenciaRef4) ? "12345687" : dados.SequenciaRef4 );
                        ReportParameter GrauPart1 = new ReportParameter("GrauPart1", string.IsNullOrEmpty(dados.GrauPart1) ? "12345687" : dados.GrauPart1 );
                        ReportParameter GrauPart2 = new ReportParameter("GrauPart2", string.IsNullOrEmpty(dados.GrauPart2) ? "12345687" : dados.GrauPart2 );
                        ReportParameter GrauPart3 = new ReportParameter("GrauPart3", string.IsNullOrEmpty(dados.GrauPart3) ? "12345687" : dados.GrauPart3 );
                        ReportParameter GrauPart4 = new ReportParameter("GrauPart4", string.IsNullOrEmpty(dados.GrauPart4) ? "12345687" : dados.GrauPart4 );
                        ReportParameter CodigoOperadoraCpf1 = new ReportParameter("CodigoOperadoraCpf1", string.IsNullOrEmpty(dados.CodigoOperadoraCpf1) ? "12345687" : dados.CodigoOperadoraCpf1 );
                        ReportParameter CodigoOperadoraCpf2 = new ReportParameter("CodigoOperadoraCpf2", string.IsNullOrEmpty(dados.CodigoOperadoraCpf2) ? "12345687" : dados.CodigoOperadoraCpf2 );
                        ReportParameter CodigoOperadoraCpf3 = new ReportParameter("CodigoOperadoraCpf3", string.IsNullOrEmpty(dados.CodigoOperadoraCpf3) ? "12345687" : dados.CodigoOperadoraCpf3 );
                        ReportParameter CodigoOperadoraCpf4 = new ReportParameter("CodigoOperadoraCpf4", string.IsNullOrEmpty(dados.CodigoOperadoraCpf4) ? "12345687" : dados.CodigoOperadoraCpf4 );
                        ReportParameter NomeProfissional1 = new ReportParameter("NomeProfissional1", string.IsNullOrEmpty(dados.NomeProfissional1) ? "12345687" : dados.NomeProfissional1 );
                        ReportParameter NomeProfissional2 = new ReportParameter("NomeProfissional2", string.IsNullOrEmpty(dados.NomeProfissional2) ? "12345687" : dados.NomeProfissional2 );
                        ReportParameter NomeProfissional3 = new ReportParameter("NomeProfissional3", string.IsNullOrEmpty(dados.NomeProfissional3) ? "12345687" : dados.NomeProfissional3 );
                        ReportParameter NomeProfissional4 = new ReportParameter("NomeProfissional4", string.IsNullOrEmpty(dados.NomeProfissional4) ? "12345687" : dados.NomeProfissional4 );
                        ReportParameter ConselhoProfissional1 = new ReportParameter("ConselhoProfissional1", string.IsNullOrEmpty(dados.ConselhoProfissional1) ? "12345687" : dados.ConselhoProfissional1 );
                        ReportParameter ConselhoProfissional2 = new ReportParameter("ConselhoProfissional2", string.IsNullOrEmpty(dados.ConselhoProfissional2) ? "12345687" : dados.ConselhoProfissional2 );
                        ReportParameter ConselhoProfissional3 = new ReportParameter("ConselhoProfissional3", string.IsNullOrEmpty(dados.ConselhoProfissional3) ? "12345687" : dados.ConselhoProfissional3 );
                        ReportParameter ConselhoProfissional4 = new ReportParameter("ConselhoProfissional4", string.IsNullOrEmpty(dados.ConselhoProfissional4) ? "12345687" : dados.ConselhoProfissional4 );
                        ReportParameter NumeroConselho1 = new ReportParameter("NumeroConselho1", string.IsNullOrEmpty(dados.NumeroConselho1) ? "12345687" : dados.NumeroConselho1 );
                        ReportParameter NumeroConselho2 = new ReportParameter("NumeroConselho2", string.IsNullOrEmpty(dados.NumeroConselho2) ? "12345687" : dados.NumeroConselho2 );
                        ReportParameter NumeroConselho3 = new ReportParameter("NumeroConselho3", string.IsNullOrEmpty(dados.NumeroConselho3) ? "12345687" : dados.NumeroConselho3 );
                        ReportParameter NumeroConselho4 = new ReportParameter("NumeroConselho4", string.IsNullOrEmpty(dados.NumeroConselho4) ? "12345687" : dados.NumeroConselho4 );
                        ReportParameter Uf1 = new ReportParameter("Uf1", string.IsNullOrEmpty(dados.Uf1) ? "12345687" : dados.Uf1 );
                        ReportParameter Uf2 = new ReportParameter("Uf2", string.IsNullOrEmpty(dados.Uf2) ? "12345687" : dados.Uf2 );
                        ReportParameter Uf3 = new ReportParameter("Uf3", string.IsNullOrEmpty(dados.Uf3) ? "12345687" : dados.Uf3 );
                        ReportParameter Uf4 = new ReportParameter("Uf4", string.IsNullOrEmpty(dados.Uf4) ? "12345687" : dados.Uf4 );
                        ReportParameter CodigoCbo1 = new ReportParameter("CodigoCbo1", string.IsNullOrEmpty(dados.CodigoCbo1) ? "12345687" : dados.CodigoCbo1 );
                        ReportParameter CodigoCbo2 = new ReportParameter("CodigoCbo2", string.IsNullOrEmpty(dados.CodigoCbo2) ? "12345687" : dados.CodigoCbo2 );
                        ReportParameter CodigoCbo3 = new ReportParameter("CodigoCbo3", string.IsNullOrEmpty(dados.CodigoCbo3) ? "12345687" : dados.CodigoCbo3 );
                        ReportParameter CodigoCbo4 = new ReportParameter("CodigoCbo4", string.IsNullOrEmpty(dados.CodigoCbo4) ? "12345687" : dados.CodigoCbo4 );
                        // Datas e Assinaturas (procedimentos em serie)
                        ReportParameter DataRealizacaoProcedimentoSerie1 = new ReportParameter("DataRealizacaoProcedimentoSerie1", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie1) ? "12345687" : dados.DataRealizacaoProcedimentoSerie1);
                        ReportParameter DataRealizacaoProcedimentoSerie2 = new ReportParameter("DataRealizacaoProcedimentoSerie2", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie2) ? "12345687" : dados.DataRealizacaoProcedimentoSerie2);
                        ReportParameter DataRealizacaoProcedimentoSerie3 = new ReportParameter("DataRealizacaoProcedimentoSerie3", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie3) ? "12345687" : dados.DataRealizacaoProcedimentoSerie3);
                        ReportParameter DataRealizacaoProcedimentoSerie4 = new ReportParameter("DataRealizacaoProcedimentoSerie4", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie4) ? "12345687" : dados.DataRealizacaoProcedimentoSerie4);
                        ReportParameter DataRealizacaoProcedimentoSerie5 = new ReportParameter("DataRealizacaoProcedimentoSerie5", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie5) ? "12345687" : dados.DataRealizacaoProcedimentoSerie5);
                        ReportParameter DataRealizacaoProcedimentoSerie6 = new ReportParameter("DataRealizacaoProcedimentoSerie6", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie6) ? "12345687" : dados.DataRealizacaoProcedimentoSerie6);
                        ReportParameter DataRealizacaoProcedimentoSerie7 = new ReportParameter("DataRealizacaoProcedimentoSerie7", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie7) ? "12345687" : dados.DataRealizacaoProcedimentoSerie7);
                        ReportParameter DataRealizacaoProcedimentoSerie8 = new ReportParameter("DataRealizacaoProcedimentoSerie8", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie8) ? "12345687" : dados.DataRealizacaoProcedimentoSerie8);
                        ReportParameter DataRealizacaoProcedimentoSerie9 = new ReportParameter("DataRealizacaoProcedimentoSerie9", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie9) ? "12345687" : dados.DataRealizacaoProcedimentoSerie9);
                        ReportParameter DataRealizacaoProcedimentoSerie10 = new ReportParameter("DataRealizacaoProcedimentoSerie10", string.IsNullOrEmpty(dados.DataRealizacaoProcedimentoSerie10) ? "12345687" : dados.DataRealizacaoProcedimentoSerie10 );
                        ReportParameter ObservacaoJustificativa = new ReportParameter("ObservacaoJustificativa", string.IsNullOrEmpty(dados.ObservacaoJustificativa) ? "12345687" : dados.ObservacaoJustificativa );
                        ReportParameter TotalProcedimentos = new ReportParameter("TotalProcedimentos", string.IsNullOrEmpty(dados.TotalProcedimentos) ? "12345687" : dados.TotalProcedimentos );
                        ReportParameter TotalTaxasAlugueis = new ReportParameter("TotalTaxasAlugueis", string.IsNullOrEmpty(dados.TotalTaxasAlugueis) ? "12345687" : dados.TotalTaxasAlugueis );
                        ReportParameter TotalMateriais = new ReportParameter("TotalMateriais", string.IsNullOrEmpty(dados.TotalMateriais) ? "12345687" : dados.TotalMateriais );
                        ReportParameter TotalOpme = new ReportParameter("TotalOpme", string.IsNullOrEmpty(dados.TotalOpme) ? "12345687" : dados.TotalOpme );
                        ReportParameter TotalMedicamentos = new ReportParameter("TotalMedicamentos",string.IsNullOrEmpty(dados.TotalMedicamentos) ? "12345687" : dados.TotalMedicamentos );
                        ReportParameter TotalGeral = new ReportParameter("TotalGeral",string.IsNullOrEmpty(dados.TotalGeral) ? "12345687" : dados.TotalGeral );



                        GuiaSpsadt.LocalReport.SetParameters(new ReportParameter[] {
                          //   Titulo ,
                             NumeroGuiaPrestador ,
                             // Guia
                             RegistroAns ,
                             NumeroGuiaPrincipal ,
                             DataAutorizacao ,
                             Senha ,
                      //       DataValidadeSenha ,
                     //        NumeroGuiaOperadora ,
                             NumeroCarteira ,
                             ValidadeCarteira ,
                   //          NomePaciente ,
                      //       NumeroCns ,
                             AtendimentoRn ,
                             CodigoOperadora ,
                             NomeContratado ,
                             NomeProfissionalSolicitante ,
                      //       ConselhoProfissional ,
                             NumeroConselho ,
                             UF ,
                             CodigoCbo ,
                             AtendimentoRn,
                         //    AssinaturaProfissionalSolicitante ,
                         //    CaraterAtendimento ,
                       //      DataSolicitacao ,
                      //       IndicacaoClinica ,
                        //     CodigoCne ,
                          //   TipoAtendimento ,
                            // IndicacaoAcidente ,
                          //   TipoConsulta ,
                         //    MotivoEncerramentoAtendimento ,
                             // Identificacao Equipe
                             //SequenciaRef1 ,
                             //SequenciaRef2 ,
                             //SequenciaRef3 ,
                             //SequenciaRef4 ,
                             //GrauPart1 ,
                             //GrauPart2 ,
                             //GrauPart3 ,
                             //GrauPart4 ,
                             //CodigoOperadoraCpf1 ,
                             //CodigoOperadoraCpf2 ,
                             //CodigoOperadoraCpf3 ,
                             //CodigoOperadoraCpf4 ,
                             //NomeProfissional1 ,
                             //NomeProfissional2 ,
                             //NomeProfissional3 ,
                             //NomeProfissional4 ,
                             //ConselhoProfissional1 ,
                             //ConselhoProfissional2 ,
                             //ConselhoProfissional3 ,
                             //ConselhoProfissional4 ,
                             //NumeroConselho1 ,
                             //NumeroConselho2 ,
                             //NumeroConselho3 ,
                             //NumeroConselho4 ,
                             //Uf1 ,
                             //Uf2 ,
                             //Uf3 ,
                             //Uf4 ,
                             //CodigoCbo1 ,
                             //CodigoCbo2 ,
                             //CodigoCbo3 ,
                             //CodigoCbo4 ,
                             //// Datas e Assinaturas (procedimentos em serie)
                             //DataRealizacaoProcedimentoSerie1 ,
                             //DataRealizacaoProcedimentoSerie2 ,
                             //DataRealizacaoProcedimentoSerie3 ,
                             //DataRealizacaoProcedimentoSerie4 ,
                             //DataRealizacaoProcedimentoSerie5 ,
                             //DataRealizacaoProcedimentoSerie6 ,
                             //DataRealizacaoProcedimentoSerie7 ,
                             //DataRealizacaoProcedimentoSerie8 ,
                             //DataRealizacaoProcedimentoSerie9 ,
                             //DataRealizacaoProcedimentoSerie10 ,
                             //ObservacaoJustificativa ,
                             //TotalProcedimentos ,
                             //TotalTaxasAlugueis ,
                             //TotalMateriais ,
                             //TotalOpme ,
                             //TotalMedicamentos ,
                             //TotalGeral
                        });

                        //fonte de dados para o relatório - datasource
                        GuiaSpsadt.LocalReport.DataSources.Clear();

                        Guias relDS = new Guias();

                        DataTable tabela = this.ConvertToDataTable(dados.Contas/*Contas temp*/, relDS.Tables["Spsadt"]);
                        ReportDataSource dataSource = new ReportDataSource("Spsadt", tabela);
                        //dataSource.Value = tabela;
                        //dataSource.Name = "Spsadt";

                        GuiaSpsadt.LocalReport.DataSources.Add(dataSource);

                        GuiaSpsadt.LocalReport.Refresh();



                        // Teste impressao direta
                        Warning[] warnings;
                        string[] streamIds;
                        string mimeType = string.Empty;
                        string encoding = string.Empty;
                        string extension = "pdf";

                        byte[] bytes = GuiaSpsadt.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

                        string pathPdf = HttpContext.Current.Server.MapPath("~");
                        pathPdf = string.Concat(pathPdf, @"Relatorios\Faturamento\Guias\teste.pdf");
                        System.IO.File.WriteAllBytes(pathPdf, bytes);


                        Response.Buffer = true;
                        Response.Clear();
                        Response.ContentType = mimeType;
                        Response.AddHeader("content-dispostion", "attachment; filename=" + pathPdf);
                        //  Response.BinaryWrite(bytes);
                        Response.Flush();
                        // Fim - teste impressao pdf
                        this.ViewBag.teste = bytes;
                        this.ViewData["teste"] = bytes as byte[];
                        

                        GuiaSpsadt.Visible = true;
                    }
                }

                public delegate void BookmarkNavigationEventHandler (
                     object sender,
                     BookmarkNavigationEventArgs e
                 );

                /// <summary>
                /// Converte lista de objetos em tabela
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <returns></returns>
                public DataTable ConvertToDataTable<T> (IList<T> data, DataTable table)
                {
                    PropertyDescriptorCollection properties =
                       TypeDescriptor.GetProperties(typeof(T));
                    //DataTable table = new DataTable();
                    //foreach (PropertyDescriptor prop in properties)
                    //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    if (data != null)
                    {
                        //var x = table.NewRow();
                        //x["RegistroAns"] = "teste";
                        //table.Rows.Add(x);
                        //foreach (T item in data)
                        //{
                        //    try
                        //    {
                        //        DataRow row = table.NewRow();
                        //        foreach (PropertyDescriptor prop in properties)
                        //            row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                        //        table.Rows.Add(row);
                        //    }
                        //    catch { }
                        //}
                    }

                    return table;

                }
            </script>
            <rsweb:ReportViewer ID="GuiaSpsadt" runat="server" ProcessingMode="Local" ShowBackButton="False" AsyncRendering="false" ShowPageNavigationControls="True" ShowFindControls="True" ShowPrintButton="True" ShowExportControls="True" Width="100%" Height="100%">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
