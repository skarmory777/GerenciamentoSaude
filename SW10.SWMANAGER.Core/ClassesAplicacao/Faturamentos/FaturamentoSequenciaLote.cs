using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos
{
    [Table("FatSequenciaLote")]
    public class FaturamentoSequenciaLote : CamposPadraoCRUD
    {
        public long Sequencia { get; set; }

        public long ConvenioId { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }
    }
}
