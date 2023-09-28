using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.AgendamentoConsultas
{
    [Table("AteAgendamentoStatus")]
    public class AgendamentoStatus : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }
    }
}
