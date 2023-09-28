using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.AtendimentosLeitosMov
{
    [Table("AteAtendimentoLeitoMov")]
    public class AtendimentoLeitoMov : CamposPadraoCRUD
    {
        [Index("Ate_Idx_DataInicial")]
        [DataType(DataType.DateTime)]
        public DateTime? DataInicial { get; set; }

        [Index("Ate_Idx_DataFinal")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFinal { get; set; }

        [Index("Ate_Idx_DataInclusao")]

        [DataType(DataType.DateTime)]
        public DateTime? DataInclusao { get; set; }

        [ForeignKey("User")]
        public long? UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long? AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }

        [ForeignKey("Leito"), Column("AteLeitoId")]
        public long? LeitoId { get; set; }
        public Leito Leito { get; set; }
    }
}
