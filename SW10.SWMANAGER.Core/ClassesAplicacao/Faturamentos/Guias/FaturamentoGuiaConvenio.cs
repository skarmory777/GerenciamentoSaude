using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos
{
    [Table("FatGuiaConvenio")]
    public class FaturamentoGuiaConvenio : CamposPadraoCRUD
    {
        [ForeignKey("Convenio"), Column("ConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Guia"), Column("GuiaId")]
        public long? GuiaId { get; set; }
        public FaturamentoGuia Guia { get; set; }
    }

}


