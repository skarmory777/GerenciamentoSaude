using SW10.SWMANAGER.ClassesAplicacao.Cadastros.VersoesTiss;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios
{
    [Table("SisConvenioURLServico")]
    public class ConvenioURLServico : CamposPadraoCRUD
    {
        public string Url { get; set; }

        [ForeignKey("Convenio"), Column("SisConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("VersaoTiss"), Column("SisVersaoTissId")]
        public long? VersaoTissId { get; set; }
        public VersaoTiss VersaoTiss { get; set; }
    }
}
