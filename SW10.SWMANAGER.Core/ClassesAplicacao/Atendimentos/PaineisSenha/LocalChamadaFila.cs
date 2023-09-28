using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteLocalChamadaFila")]
    public class LocalChamadaFila : CamposPadraoCRUD
    {
        public long? FilaId { get; set; }
        public long? LocalChamadaId { get; set; }

        [ForeignKey("FilaId")]
        public Fila Fila { get; set; }

        [ForeignKey("LocalChamadaId")]
        public LocalChamada LocalChamada { get; set; }
    }
}
