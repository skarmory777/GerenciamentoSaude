using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosEstoque.Dto
{
    [AutoMap(typeof(ProdutoEstoque))]
    public class ProdutoEstoqueDto : CamposPadraoCRUDDto
    {
        /// <Summary>
        /// Produto
        /// </Summary>
        public long ProdutoId { get; set; }

        /// <summary>
        /// Estoque
        /// </summary>
        public virtual Estoque Estoque { get; set; }
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