using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssSolicitacaoExamePrioridade")]
    public class SolicitacaoExamePrioridade : CamposPadraoCRUD
    {
        public static long Rotina = 1;
        public static long Urgente = 2;
        [StringLength(30)]
        public override string Descricao { get; set; }
    }
}
