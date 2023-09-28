using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteLocalChamada")]
    public class LocalChamada : CamposPadraoCRUD
    {
        public long? TipoLocalChamadaId { get; set; }

        [ForeignKey("TipoLocalChamadaId")]
        public TipoLocalChamada TipoLocalChamada { get; set; }
    }
}
