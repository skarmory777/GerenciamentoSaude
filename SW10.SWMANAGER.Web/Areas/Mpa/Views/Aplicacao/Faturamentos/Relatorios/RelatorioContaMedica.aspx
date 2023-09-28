<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Faturamentos.Relatorios" %>
<!-- namespace para localizar FiltroModel-->
<!-- namespace para localizar o DataSet -->

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
                private void Page_Load (object sender, System.EventArgs e)
                {
                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
                    scriptManager.RegisterPostBackControl(this.ReportViewer1);

                    // Localização do relatório
                    var dados = (FiltroModel)Model;
                    string path = HttpContext.Current.Server.MapPath("~");
                    ReportViewer1.LocalReport.ReportPath = string.Concat(path, @"Relatorios\Faturamento\CM.rdlc");

                    if (dados != null)
                    {
                        // Parâmetros do relatório
                        ReportViewer1.LocalReport.SetParameters(new ReportParameter[] {
                            new ReportParameter("Empresa",        dados.NomeHospital),
                            new ReportParameter("ContaMedica",    dados.Titulo),
                            new ReportParameter("Usuario",        dados.NomeUsuario),
                            new ReportParameter("DataHora",       dados.DataHora),
                            new ReportParameter("Paciente",       dados.Paciente),
                            new ReportParameter("Nascimento",     dados.Nascimento)
                            ,
                            new ReportParameter("Convenio",       dados.Convenio),
                            new ReportParameter("Plano",          dados.Plano),
                            new ReportParameter("Matricula",      dados.Matricula),
                            new ReportParameter("Titular",        dados.Titular),
                            new ReportParameter("ValidCarteira",  dados.ValidCarteira),
                            new ReportParameter("DataInternacao", dados.DataInternacao),
                            new ReportParameter("Senha",          dados.Senha),
                            new ReportParameter("Guia",           dados.Guia),
                            new ReportParameter("Especialidade",  dados.Especialidade),
                            new ReportParameter("Medico",         dados.Medico),
                            new ReportParameter("CRM",            dados.CRM),
                            new ReportParameter("TipoAlta",       dados.TipoAlta)
                        });

                        // Fonte de dados para o relatório - datasource

                        SW10.SWMANAGER.Web.Relatorios.Faturamento.ContaMedicaDS relDS = new SW10.SWMANAGER.Web.Relatorios.Faturamento.ContaMedicaDS();
                        //   ContaMedicaDS relDS = new  ContaMedicaDS();

                        // Iterando pelos milhares de dicionarios
                        foreach (var i in dados.ListaTotal)
                        {
                            DataTable tab = this.DicionarioParaTabela(i.Value, relDS.Tables["ItemDataTable"], i.Key);
                            ReportDataSource dataSource = new ReportDataSource();
                            dataSource.Value = tab;
                            dataSource.Name = "ItemDataSet";
                            //ReportViewer1.LocalReport.DataSources.Clear();
                            ReportViewer1.LocalReport.DataSources.Add(dataSource);
                        }

                        ReportViewer1.LocalReport.Refresh();
                        ReportViewer1.Visible = true;
                    }
                }

                public delegate void BookmarkNavigationEventHandler (object sender, BookmarkNavigationEventArgs e);

                //public DataTable ConvertToDataTable<T> (IList<T> data, DataTable table)
                //{
                //    PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));

                //    if (data != null)
                //    {
                //        foreach (T item in data)
                //        {
                //            DataRow row = table.NewRow();
                //            foreach (PropertyDescriptor prop in properties)
                //            {
                //                try
                //                {
                //                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                //                }
                //                catch { }
                //            }

                //            table.Rows.Add(row);
                //        }
                //    }
                //    return table;
                //}

                public DataTable DicionarioParaTabela (IList<Dictionary<string, string>> data, DataTable table, string titulo)
                {
                    if (data != null)
                    {
                        DataRow rowTitulo = table.NewRow();
                        rowTitulo["Descricao"] = titulo;
                        table.Rows.Add(rowTitulo);

                        // Linha vazia
                        DataRow rowLinhaVazia = table.NewRow();
                        table.Rows.Add(rowLinhaVazia);

                        foreach (var item in data)
                        {
                            if (item == null)
                                continue;

                            DataRow row = table.NewRow();
                            bool medicoRow = false;
                            string medicoNome = string.Empty;

                            foreach (var prop in item)
                            {
                                if (prop.Key.Equals("Medico") && !string.IsNullOrEmpty(prop.Value))
                                {
                                    medicoRow = true;
                                    medicoNome = prop.Value;
                                }
                                else
                                {
                                    try
                                    {
                                        row[prop.Key] = prop.Value;

                                        if (prop.Key.Equals("Percentual"))
                                        {
                                            row[prop.Key] += "%";
                                        }
                                    }
                                    catch (Exception ex) { ex.ToString(); }
                                }

                            }

                            table.Rows.Add(row);

                            if (medicoRow)
                            {
                                DataRow rowMedico = table.NewRow();
                                rowMedico["Codigo"] = "Médico";
                                rowMedico["Descricao"] = medicoNome;
                                table.Rows.Add(rowMedico);
                            }
                        }

                        // Linha vazia
                        DataRow rowLinhaVaziaFim = table.NewRow();
                        table.Rows.Add(rowLinhaVaziaFim);
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
