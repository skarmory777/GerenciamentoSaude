using Abp.Application.Services;
using System.Collections.Generic;
using System.IO;

namespace SW10.SWMANAGER.DataExporting.HtmlParaImagem
{
    public interface IHtmlToPDF : IApplicationService
    {
        byte[] ConvertHtmlToPDF(StringReader srtringReaderHTML);

        byte[] ConcatAndAddContent(List<byte[]> pdfByteContent);
    }
}
