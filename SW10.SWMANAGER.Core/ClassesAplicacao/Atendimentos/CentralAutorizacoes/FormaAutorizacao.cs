using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes
{
    [Table("AteFormaAutorizacao")]
    public class FormaAutorizacao : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }
    }
}
