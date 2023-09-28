using System.ComponentModel.DataAnnotations.Schema;
namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Entradas
{
    [Table("EntradaItem")]
    public class EntradaItem : CamposPadraoCRUD
    {
        public long EntradaId { get; set; }
        public long ProdutoId { get; set; }
        public long Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }

        [ForeignKey("EntradaId")]
        public Entrada Entrada { get; set; }
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }


    }
}
