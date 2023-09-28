using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    [Table("AteMotivoCancelamento")]
    public class AtendimentoMotivoCancelamento : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }
    }
}
