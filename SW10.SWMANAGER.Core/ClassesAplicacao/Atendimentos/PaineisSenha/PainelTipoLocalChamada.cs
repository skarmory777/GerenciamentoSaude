using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AtePainelTipoLocalChamada")]
    public class PainelTipoLocalChamada : CamposPadraoCRUD
    {
        public long? TipoLocalChamadaId { get; set; }
        public long? PainelId { get; set; }

        [ForeignKey("TipoLocalChamadaId")]
        public TipoLocalChamada TipoLocalChamada { get; set; }

        [ForeignKey("PainelId")]
        public Painel Painel { get; set; }
    }
}
