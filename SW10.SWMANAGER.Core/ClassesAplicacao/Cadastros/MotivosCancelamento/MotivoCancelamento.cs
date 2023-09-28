using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.MotivosCancelamento
{
    [Table("MotivoCancelamento")]
    public class MotivoCancelamento : CamposPadraoCRUD
    {
        public bool IsAtivo { get; set; }
    }
}