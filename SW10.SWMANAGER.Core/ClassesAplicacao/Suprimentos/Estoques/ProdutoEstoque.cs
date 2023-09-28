using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_ProdutoEstoque")]
    public class ProdutoEstoque : CamposPadraoCRUD
    {
        /// <Summary>
        /// Produto
        /// </Summary>
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        public long ProdutoId { get; set; }

        /// <summary>
        /// Estoque
        /// </summary>
        [ForeignKey("EstoqueId")]
        public Estoque Estoque { get; set; }
        public long EstoqueId { get; set; }

        /// <summary>
        /// Estoque Minimo
        /// </summary>
        public long EstoqueMinimo { get; set; }

        /// <summary>
        /// Estoque Maximo
        /// </summary>
        public long EstoqueMaximo { get; set; }

        /// <summary>
        /// Ponto de Pedido
        /// </summary>
        public long PontoPedido { get; set; }

    }
}
