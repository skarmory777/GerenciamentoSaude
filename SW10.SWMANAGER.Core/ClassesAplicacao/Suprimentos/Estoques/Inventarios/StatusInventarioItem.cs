using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios
{
    [Table("EstStatusInventarioItem")]
    public class StatusInventarioItem : CamposPadraoCRUD
    {
        public static long Inicial = 1;
        public static long PrimeiraContagem = 2;
        public static long SegundaContagem = 3;
        public static long Fechado = 4;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }
    }
}
