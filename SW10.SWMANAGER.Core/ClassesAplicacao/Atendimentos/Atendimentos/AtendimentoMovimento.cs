using Abp.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    //Definição da tabela feita pelo George.
    [Table("AssAtendimentoMovimento")]
    public class AtendimentoMovimento:CamposPadraoCRUD
    {
        public long AtendimentoId { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }

        [Index("Ate_Idx_DataInicio")]
        public DateTime? DataInicio { get; set; }

        [Index("Ate_Idx_DataFinal")]
        public DateTime? DataFinal { get; set; }

        public long? MedicoId { get; set; }

        [ForeignKey("MedicoId")]
        public Medico Medico { get; set; }
    }
}
