<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Web.Mvc" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.ComponentModel" %>
<%@ Import Namespace="SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.Relatorios" %> <!-- namespace para localizar FiltroModel-->
<%@ Import Namespace="SW10.SWMANAGER.Web.Relatorios.Atendimento" %> <!-- namespace para localizar o DataSet -->


<%@ Page Language="C#" EnableEventValidation="false" Inherits="System.Web.Mvc.ViewPage" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

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
                private void Page_Load(object sender, System.EventArgs e)
                {

                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(this.ReportViewer1);


                    //localização do relatório
                    var dados = (FiltroModel)Model;
                    string path = HttpContext.Current.Server.MapPath("~");
                    ReportViewer1.LocalReport.ReportPath = string.Concat(path, @"Relatorios\Atendimento\RelatorioInternado.rdlc");

                    if (dados != null)
                    {
                        //parâmetros para o relatório
                        ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                        ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                        ReportParameter usuario = new ReportParameter("Usuario", dados.NomeUsuario);
                        ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);
                        ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { nomeHospital, titulo, usuario, dataHora });

                        //fonte de dados para o relatório - datasource
                        //RelatorioInternadoDS relDS = new RelatorioInternadoDS();
                        Atendimento relDS = new Atendimento();
                        DataTable tabela = this.ConvertToDataTable(dados.Lista, relDS.Tables["AtendimentoDS"]);
                        ReportDataSource dataSource = new ReportDataSource();
                        dataSource.Value = tabela;
                        dataSource.Name = "RelatorioInternado";
                        ReportViewer1.LocalReport.DataSources.Clear();

                        ReportViewer1.LocalReport.DataSources.Add(dataSource);
                        ReportViewer1.LocalReport.Refresh();

                        ReportViewer1.Visible = true;
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
                    if(data != null){
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
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Local" ShowBackButton="False" AsyncRendering="false" ShowPageNavigationControls="True" ShowFindControls="True" ShowPrintButton="True" ShowExportControls="True" Width="100%" Height="100%">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
