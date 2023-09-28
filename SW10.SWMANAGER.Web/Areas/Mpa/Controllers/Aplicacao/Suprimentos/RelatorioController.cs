using System.Web.Mvc;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Controllers.Aplicacao.Suprimentos
{
    public class RelatorioController : Controller
    {
        // GET: Mpa/Relatorio
        public ActionResult Index()
        {
            Views.Aplicacao.Suprimentos.Estoques.SWMANAGERDataSet ds = new Views.Aplicacao.Suprimentos.Estoques.SWMANAGERDataSet();
            Views.Aplicacao.Suprimentos.Estoques.SWMANAGERDataSetTableAdapters.Est_ProdutoTableAdapter tableAdapter = new Views.Aplicacao.Suprimentos.Estoques.SWMANAGERDataSetTableAdapters.Est_ProdutoTableAdapter();
            tableAdapter.Fill(ds.Est_Produto);

            Microsoft.Reporting.WebForms.ReportViewer viewer = new Microsoft.Reporting.WebForms.ReportViewer();
            viewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
            viewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + "\\Areas\\Mpa\\Views\\Aplicacao\\Suprimentos\\Estoques\\RelatorioProdutos.rdlc";
            viewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("DataSet1", (System.Data.DataTable)ds.Est_Produto));



            ViewBag.ReportViewer = viewer;

            // Variables
            Microsoft.Reporting.WebForms.Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            byte[] bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);


            //call service and get data
            //string fileContent = response.FileContent;
            //byte[] data = Convert.FromBase64String(fileContent);
            byte[] data = bytes;

            return File(data, "application/pdf");

            //using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
            //{
            //    Response.ClearHeaders();
            //    Response.ClearContent();
            //    Response.Charset = "";
            //    Response.AddHeader("Content-Type", "application/pdf");
            //    memoryStream.Write(data, 0, data.Length);
            //    memoryStream.WriteTo(Response.OutputStream);
            //    Response.Flush();
            //    Response.Close();
            //    Response.End();
            //}


            //return View();
        }
    }
}