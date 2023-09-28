using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Pessoas
{
    [Table("SisTipoPessoa")]
    public class SisTipoPessoa : CamposPadraoCRUD
    {
        public bool IsReceber { get; set; }
        public bool IsPagar { get; set; }
        public bool IsAtivo { get; set; }
    }
}
