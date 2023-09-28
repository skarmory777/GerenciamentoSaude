using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("Est_ProdutoUnidade")]
    public class ProdutoUnidade : CamposPadraoCRUD
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
        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
        public long ProdutoId { get; set; }

        /// <summary>
        /// Unidade relacionada ao produto
        /// </summary>
        [ForeignKey("UnidadeId")]
        public Unidade Unidade { get; set; }
        public long UnidadeId { get; set; }

        /// <summary>
        /// Tipo de unidade para o produto relacionado
        /// (Referência, Gerencial, Compras, Entreda, Estoque)
        /// </summary>
        [ForeignKey("UnidadeTipoId")]
        public UnidadeTipo Tipo { get; set; }
        public long UnidadeTipoId { get; set; }
    }
}
