using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    [Table("CmpRequisicaoItem")]
    public class CompraRequisicaoItem : CamposPadraoCRUD
    {
        [ForeignKey("Requisicao"), Column("CmpRequisicaoId")]
        public long RequisicaoId { get; set; }
        public CompraRequisicao Requisicao { get; set; }

        /// <summary>
        /// Indica o modo de inclusao individual do item de Requisicao
        /// Ex.: Pode ser feita uma Requisicao Automatica, e todos os produtos retornados pela Req Aut serão marcados com "A", porém além destes, pode-se incluir produtos manualmente, marcados com "M"
        /// </summary>
        [StringLength(1, MinimumLength = 1)]
        public string ModoInclusao { get; set; }

        #region Item Requisicao
        [ForeignKey("Produto"), Column("EstProdutoId")]
        public long ProdutoId { get; set; }
        public Produto Produto { get; set; }

        /// <summary>
        /// Unidade relacionada ao produto
        /// </summary>
        [ForeignKey("Unidade"), Column("UnidadeId")]
        public long UnidadeId { get; set; }
        public Unidade Unidade { get; set; }

        /// <summary>
        /// Quantidade requisitada do produto, sujeita a aprovação
        /// </summary>
        public decimal Quantidade { get; set; }
        #endregion

        #region Aprovacao
        [ForeignKey("ProdutoAprovacao"), Column("EstProdutoAprovacaoId")]
        public long? ProdutoAprovacaoId { get; set; }
        public Produto ProdutoAprovacao { get; set; }

        /// <summary>
        /// Unidade relacionada ao produto
        /// </summary>
        [ForeignKey("UnidadeAprovacao"), Column("UnidadeAprovacaoId")]
        public long? UnidadeAprovacaoId { get; set; }
        public Unidade UnidadeAprovacao { get; set; }

        /// <summary>
        /// Quantidade requisitada do produto, sujeita a aprovação
        /// </summary>
        public decimal? QuantidadeAprovacao { get; set; }
        #endregion
    }
}
