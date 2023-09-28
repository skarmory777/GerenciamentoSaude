using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.TiposAtendimento
{
    [Table("AteAtendimentoTipo")]
    public class TipoAtendimento : CamposPadraoCRUD
    {
        public bool IsInternacao { get; set; }

        public bool IsAmbulatorioEmergencia { get; set; }

        [Column("SisTabelaItemTissId")]
        public long? TabelaItemTissId { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominio TabelaDominio { get; set; }
    }
}
