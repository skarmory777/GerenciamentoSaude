using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosUnidade.Dto
{
    [AutoMap(typeof(ProdutoUnidade))]
    public class CriarOuEditarProdutoUnidade : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Se estar disponível para uso
        /// </summary>
        public bool IsAtivo { get; set; }

        /// <summary>
        /// Se pode usar este produto em prescrição médica
        /// </summary>
        public bool IsPrescricao { get; set; }

        /// <summary>
        /// Produto relacionado a unidade
        /// </summary>
        public long ProdutoId { get; set; }

        /// <summary>
        /// Unidade relacionada ao produto
        /// </summary>
        public long UnidadeId { get; set; }

        /// <summary>
        /// Tipo de unidade para o produto relacionado
        /// (Referência, Gerencial, Compras, Entreda, Estoque)
        /// </summary>
        public long UnidadeTipoId { get; set; }

    }
}
