using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteSenha")]
    public class Senha : CamposPadraoCRUD
    {
        [Index("Ate_Idx_DataHora")]
        public DateTime DataHora { get; set; }
        public int Numero { get; set; }
        public long FilaId { get; set; }
        public long? AtendimentoId { get; set; }

        [ForeignKey("FilaId")]
        public Fila Fila { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }
    }
}
