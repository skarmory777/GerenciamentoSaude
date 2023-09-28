using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace SW10.SWMANAGER.DataExporting.HtmlParaImagem
{
    public class HtmlToPDF : SWMANAGERServiceBase, IHtmlToPDF
    {
        public byte[] ConvertHtmlToPDF(StringReader srtringReaderHTML)
        {
            try
            {
                byte[] bytes;
                var pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

                using (var memoryStream = new MemoryStream())
                {
                    var htmlparser = new HTMLWorker(pdfDoc);
                    PdfWriter.GetInstance(pdfDoc, memoryStream);
                    pdfDoc.Open();

                    htmlparser.Parse(srtringReaderHTML);
                    pdfDoc.Close();

                    bytes = memoryStream.ToArray();
                    memoryStream.Close();
                }

                return bytes;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public byte[] ConcatAndAddContent(List<byte[]> pdfByteContent)
        {

            using (var ms = new MemoryStream())
            {
                using (var doc = new Document())
                {
                    using (var copy = new PdfSmartCopy(doc, ms))
                    {
                        doc.Open();

                        //Loop through each byte array
                        foreach (var p in pdfByteContent)
                        {

                            //Create a PdfReader bound to that byte array
                            using (var reader = new PdfReader(p))
                            {

                                //Add the entire document instead of page-by-page
                                copy.AddDocument(reader);
                            }
                        }

                        doc.Close();
                    }
                }

                //Return just before disposing
                return ms.ToArray();
            }
        }
    }
}
