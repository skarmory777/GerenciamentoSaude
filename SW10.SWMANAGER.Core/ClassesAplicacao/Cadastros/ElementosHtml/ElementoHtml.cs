using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml
{
    [Table("SisElementoHtml")]
    public class ElementoHtml : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("ElementoHtmlTipo"), Column("SisElementoHtmlTipoId")]
        public long? ElementoHtmlTipoId { get; set; }

        public int Tamanho { get; set; }

        public bool IsRequerido { get; set; }

        public bool IsDesativado { get; set; }

        public ElementoHtmlTipo ElementoHtmlTipo { get; set; }
    }
}
