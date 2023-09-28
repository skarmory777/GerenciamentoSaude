using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinAgencia")]
    public class Agencia : CamposPadraoCRUD
    {
        public long? BancoId { get; set; }

        [ForeignKey("BancoId")]
        public Banco Banco { get; set; }
    }
}
