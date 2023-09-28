using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Inventarios
{
    [Table("EstStatusInventario")]
    public class StatusInventario : CamposPadraoCRUD
    {
        public const long Inicial = 1;
        public const long Fechado = 4;

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }
    }
}
