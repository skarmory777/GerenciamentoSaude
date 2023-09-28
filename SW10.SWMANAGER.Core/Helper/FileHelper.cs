using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace SW10.SWMANAGER.Helper
{
    public static class FileHelper
    {
        public static byte[] ConcatAndAddContent(List<byte[]> pdfByteContent)
        {
            if (!pdfByteContent.Any())
            {
                return null;
            }

            using (var ms = new MemoryStream())
            using (var doc = new Document())
            using (var copy = new PdfSmartCopy(doc, ms))
            {
                doc.Open();
                foreach (var p in pdfByteContent)
                {
                    try
                    {
                        using (var reader = new PdfReader(p))
                        {
                            copy.AddDocument(reader);
                        }
                    }
                    catch { }
                }
                doc.Close();
                return ms.ToArray();
            }
        }

        public static Stream GenerateStreamFromString(this StringBuilder sb)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(sb.ToString());
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static byte[] GenerateByteFromString(this StringBuilder sb)
        {
            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}