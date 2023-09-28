using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteSenhaMov")]
    public class SenhaMovimentacao : CamposPadraoCRUD
    {
        public long SenhaId { get; set; }
        public long? LocalChamadaId { get; set; }
        public long? TipoLocalChamadaId { get; set; }

        [Index("Ate_Idx_DataHora")]

        public DateTime DataHora { get; set; }

        [Index("Ate_Idx_DataHoraInicial")]
        public DateTime? DataHoraInicial { get; set; }

        [Index("Ate_Idx_DataHoraFinal")]
        public DateTime? DataHoraFinal { get; set; }

        [ForeignKey("SenhaId")]
        public Senha Senha { get; set; }

        [ForeignKey("LocalChamadaId")]
        public LocalChamada LocalChamada { get; set; }

        [ForeignKey("TipoLocalChamadaId")]
        public TipoLocalChamada TipoLocalChamada { get; set; }

    }
}
