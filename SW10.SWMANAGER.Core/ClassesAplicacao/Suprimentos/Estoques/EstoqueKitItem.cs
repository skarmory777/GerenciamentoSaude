using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("EstKitItem")]
    public class EstoqueKitItem : CamposPadraoCRUD
    {
        public long? EstoqueKitId { get; set; }

        [ForeignKey("EstoqueKitId")]
        public EstoqueKit EstoqueKit { get; set; }

        public int Quantidade { get; set; }

        public long ProdutoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        public long? UnidadeId { get; set; }

        [ForeignKey("UnidadeId")]
        public Unidade Unidade { get; set; }
    }
}
