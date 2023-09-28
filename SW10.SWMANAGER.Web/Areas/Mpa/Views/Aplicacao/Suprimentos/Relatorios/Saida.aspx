<%@ Import Namespace="SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Suprimentos.Estoques.Relatorios" %>

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
                    var dados = (RelatorioEntradaModel)Model;

                    ReportViewer1.LocalReport.ReportPath = "Relatorios/Suprimento/Estoque/Saida.rdlc";
                    ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", dados.Itens));

                    ReportParameter nomeHospital = new ReportParameter("NomeHospital", dados.NomeHospital);
                    ReportParameter titulo = new ReportParameter("Titulo", dados.Titulo);
                    ReportParameter usuario = new ReportParameter("Usuario", dados.NomeUsuario);
                    ReportParameter dataHora = new ReportParameter("DataHora", dados.DataHora);

                    ReportParameter documento = new ReportParameter("documento", dados.Documento);
                    ReportParameter estoque = new ReportParameter("Estoque", dados.Estoque);
                    ReportParameter dataEntrada = new ReportParameter("DataEntrada", dados.DataEntrada);
                    ReportParameter tipoEntrada = new ReportParameter("TipoEntrada", dados.TipoEntrada);
                    ReportParameter usuarioEntrada = new ReportParameter("UsuarioEntrada", dados.UsuarioEntrada);

                    ReportParameter paciente = new ReportParameter("Paciente", dados.Paciente);
                    ReportParameter setor = new ReportParameter("Setor", dados.Setor);

                    ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { nomeHospital
                                                                                  , titulo
                                                                                  , usuario
                                                                                  , dataHora
                                                                                  , paciente
                                                                                  , documento
                                                                                  , setor
                                                                                  , estoque
                                                                                  , dataEntrada
                                                                                  , usuarioEntrada
                                                                                  , tipoEntrada
                    });

                    ReportViewer1.LocalReport.Refresh();

                    ReportViewer1.Visible = true;
                }
            </script>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" ProcessingMode="Local" ShowBackButton="True" AsyncRendering="false" ShowPageNavigationControls="true" ShowFindControls="true" ShowPrintButton="true" ShowExportControls="true" Width="100%" Height="100%">
            </rsweb:ReportViewer>
        </div>
    </form>
</body>
</html>
