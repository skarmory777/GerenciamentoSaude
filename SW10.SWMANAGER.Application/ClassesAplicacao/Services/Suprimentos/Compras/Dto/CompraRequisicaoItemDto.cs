using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    [AutoMap(typeof(CompraRequisicaoItem))]
    public class CompraRequisicaoItemDto : CamposPadraoCRUDDto
    {
        /// <summary>
        /// Id da Requisicao
        /// </summary>
        public long RequisicaoId { get; set; }

        #region Item Requisicao
        /// <summary>
        /// Id do produto
        /// </summary>
        public long ProdutoId { get; set; }
        public ProdutoDto Produto { get; set; }

        /// <summary>
        /// Quantidade requisitada do produto, sujeita a aprovação
        /// </summary>
        public decimal Quantidade { get; set; }
        //public long Quantidade { get; set; }

        /// <summary>
        /// Unidade relacionada ao produto
        /// </summary>
        public long UnidadeId { get; set; }
        public UnidadeDto Unidade { get; set; }
        #endregion

        #region Item Aprovacao
        /// <summary>
        /// Id do produto
        /// </summary>
        public long? ProdutoAprovacaoId { get; set; }
        public ProdutoDto ProdutoAprovacao { get; set; }

        /// <summary>
        /// Quantidade requisitada do produto, sujeita a aprovação
        /// </summary>
        public decimal? QuantidadeAprovacao { get; set; }
        //public long Quantidade { get; set; }

        /// <summary>
        /// Unidade relacionada ao produto
        /// </summary>
        public long? UnidadeAprovacaoId { get; set; }
        public UnidadeDto UnidadeAprovacao { get; set; }
        #endregion

        /// <summary>
        /// Indica o modo de inclusao individual do item de Requisicao
        /// Ex.: Pode ser feita uma Requisicao Automatica, e todos os produtos retornados pela Req Aut serão marcados com "A", porém além destes, pode-se incluir produtos manualmente, marcados com "M"
        /// </summary>
        //[StringLength(1, MinimumLength = 1)]
        public string ModoInclusao { get; set; }

        /// <summary>
        /// Id artificial para manipulacao dinamica
        /// </summary>
        public long? IdGrid { get; set; }
    }
}
