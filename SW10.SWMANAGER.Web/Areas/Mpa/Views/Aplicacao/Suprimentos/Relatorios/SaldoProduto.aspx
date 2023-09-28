<%@ Import Namespace="SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Relatorios" %>

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
                    var dados = (FiltroModel)Model;
                    ReportViewer1.LocalReport.ReportPath = "Areas/Mpa/Views/Aplicacao/Suprimentos/Relatorios/ExtratoProdutos.rdlc";
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dados.Dados));
                    ReportViewer1.LocalReport.Refresh();

                    ReportViewer1.Visible = true;
                }
            </script>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Local" ShowBackButton="False" AsyncRendering="false" ShowPageNavigationControls="False" ShowFindControls="False" ShowPrintButton="False" ShowExportControls="false" Width="100%" Height="100%">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
