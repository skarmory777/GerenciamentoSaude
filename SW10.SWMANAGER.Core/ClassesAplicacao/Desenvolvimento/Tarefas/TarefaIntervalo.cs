using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("SisTarefaIntervalo")]
    public class TarefaIntervalo : CamposPadraoCRUD
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }

        [ForeignKey("Tarefa"), Column("SisTarefaId")]
        public long TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }

        public long? ResponsavelId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Fim { get; set; }
    }
}
