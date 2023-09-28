<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.ComponentModel" %>
<%@ Import Namespace="SW10.SWMANAGER.Web.Relatorios.Assistenciais" %>
<!-- namespace para localizar FiltroModel-->
<%@ Import Namespace="SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Assistenciais.Relatorios" %>
<!-- namespace para localizar o DataSet -->


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
    <form id="FrmPrescricaoMedica" runat="server">
        <asp:ScriptManager ID="SMPrescricaoMedica" runat="server"></asp:ScriptManager>
        <div>
            <script runat="server">
                private void Page_Load(object sender, System.EventArgs e)
                {

                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(this.RVPrescricaoMedica);


                    //localização do relatório
                    var dados = (FiltroModel)Model;
                    string path = HttpContext.Current.Server.MapPath("~");
                    RVPrescricaoMedica.LocalReport.ReportPath = string.Concat(path, @"Relatorios\Assistenciais\PrescricoesMedicas\PrescricaoMedica.rdlc");

                    if (dados != null)
                    {
                        var a = dados.Atendimento;
                        //parâmetros para o relatório
                        ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                        ReportParameter nomeUsuario = new ReportParameter("NomeUsuario", dados.NomeUsuario);
                        ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);
                        ReportParameter paciente = new ReportParameter("Paciente", dados.Paciente);
                        ReportParameter atendimento = new ReportParameter("Atendimento", dados.Atendimento);
                        ReportParameter convenio = new ReportParameter("Convenio", dados.Convenio);
                        ReportParameter internacao = new ReportParameter("Internacao", dados.Internacao);
                        ReportParameter leito = new ReportParameter("Leito", dados.Leito);
                        ReportParameter nascimento = new ReportParameter("Nascimento", dados.Nascimento);
                        ReportParameter prescricao = new ReportParameter("Prescricao", dados.Prescricao);
                        ReportParameter medico = new ReportParameter("Medico", dados.Medico);
                        ReportParameter crm = new ReportParameter("CRM", dados.CRM);
                        ReportParameter unidadeOrganizacional = new ReportParameter("unidadeOrganizacional", dados.UnidadeOrganizacional);
                        
                        RVPrescricaoMedica.LocalReport.SetParameters(new ReportParameter[] {
                            nomeHospital,
                            titulo,
                            nomeUsuario,
                            dataHora,
                            paciente,
                            atendimento,
                            convenio,
                            internacao,
                            leito,
                            nascimento,
                            prescricao,
                            medico,
                            crm,
                            unidadeOrganizacional
                        });

                        //fonte de dados para o relatório - datasource
                        //RelatorioInternadoDS relDS = new RelatorioInternadoDS();
                        Assistencial relDS = new Assistencial();
                        DataTable tabela = this.ConvertToDataTable(dados.Respostas, relDS.Tables["PrescricaoMedica"]);
                        ReportDataSource dataSource = new ReportDataSource();
                        dataSource.Value = tabela;
                        dataSource.Name = "PrescricaoMedica";
                        RVPrescricaoMedica.LocalReport.DataSources.Clear();

                        RVPrescricaoMedica.LocalReport.DataSources.Add(dataSource);
                        RVPrescricaoMedica.LocalReport.Refresh();

                        RVPrescricaoMedica.Visible = true;
                    }
                }

                public delegate void BookmarkNavigationEventHandler(
                     object sender,
                     BookmarkNavigationEventArgs e
                 );

                /// <summary>
                /// Converte lista de objetos em tabela
                /// </summary>
                /// <typeparam name="T"></typeparam>
                /// <param name="data"></param>
                /// <returns></returns>
                public DataTable ConvertToDataTable<T>(IList<T> data, DataTable table)
                {
                    PropertyDescriptorCollection properties =
                       TypeDescriptor.GetProperties(typeof(T));
                    //DataTable table = new DataTable();
                    //foreach (PropertyDescriptor prop in properties)
                    //table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    if (data != null)
                    {
                        foreach (T item in data)
                        {
                            DataRow row = table.NewRow();
                            foreach (PropertyDescriptor prop in properties)
                                row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                            table.Rows.Add(row);
                        }
                    }

                    return table;

                }
            </script>
            <rsweb:ReportViewer ID="RVPrescricaoMedica" runat="server" ProcessingMode="Local" ShowBackButton="False" AsyncRendering="false" ShowPageNavigationControls="True" ShowFindControls="True" ShowPrintButton="True" ShowExportControls="True" Width="100%" Height="100%">
            </rsweb:ReportViewer>
        </div>
        <div class="row">
            <div class="col-md-12 text-right">
                <button type="button" id="btn-imprimir" class="btn btn-default"><i class="fa fa-print"></i>Imprimir</button>
            </div>
        </div>

    </form>
    <script src="../../../../../../../libs/jquery/jquery.min.js"></script>
    <script src="../../../../../../../Scripts/MyScripts.js"></script>
    <script>
        $(document).ready(function () {
            addPrintButton("RVPrescricaoMedica");
        });
        $('#btn-imprimir').on('click', function (e) {
            e.preventDefault();
            printReport("RVPrescricaoMedica");
        });

    </script>
</body>
</html>
