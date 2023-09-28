using System.Web.Mvc;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Dto
{
    public class RetornoArquivo
    {
        public bool IsPDF { get; set; }
        public string ImgSrc { get; set; }
        public FileContentResult FilePDF { get; set; }
    }
}
