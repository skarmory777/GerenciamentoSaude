using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Guias
{
    [Table("SisGuia")]
    public class Guia : CamposPadraoCRUD
    {

        [ForeignKey("Originaria"), Column("SisOriginariaId")]
        public long? OriginariaId { get; set; }

        public byte[] ModeloPDF { get; set; }

        public string ModeloPDFMimeType { get; set; }

        public byte[] ModeloPNG { get; set; }

        public string ModeloPNGMimeType { get; set; }

        public string CamposJson { get; set; }

        public Guia Originaria { get; set; }
    }
}

