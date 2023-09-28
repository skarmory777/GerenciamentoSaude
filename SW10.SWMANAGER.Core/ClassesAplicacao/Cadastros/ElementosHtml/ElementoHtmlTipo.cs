using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml
{
    [Table("SisElementoHtmlTipo")]
    public class ElementoHtmlTipo : CamposPadraoCRUD, IDescricao
    {
        public string HtmlHelper { get; set; }
    }
}
