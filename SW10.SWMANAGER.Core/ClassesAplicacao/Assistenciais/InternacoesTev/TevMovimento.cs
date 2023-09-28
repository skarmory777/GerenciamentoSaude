using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.InternacoesTev
{
    [Table("AssTevMovimento")]
    public class TevMovimento : CamposPadraoCRUD
    {
        public DateTime Data { get; set; }

        [ForeignKey("Risco"), Column("AssTevRiscoId")]
        public long? RiscoId { get; set; }

        public string Observacao { get; set; }

        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long? AtendimentoId { get; set; }
        public Atendimento Atendimento { get; set; }

        public TevRisco Risco { get; set; }
    }
}
