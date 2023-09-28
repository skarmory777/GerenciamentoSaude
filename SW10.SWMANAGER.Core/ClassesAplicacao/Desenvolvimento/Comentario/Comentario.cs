using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [Table("SisComentario")]
    public class Comentario : CamposPadraoCRUD
    {
        public long? UsuarioId { get; set; }

        [ForeignKey("Tarefa"), Column("SisTarefaId")]
        public long TarefaId { get; set; }
        public Tarefa Tarefa { get; set; }

        [Index("Sis_Idx_DataRegistro")]
        [DataType(DataType.DateTime)]
        public DateTime? DataRegistro { get; set; }

        public string Conteudo { get; set; }
    }
}
