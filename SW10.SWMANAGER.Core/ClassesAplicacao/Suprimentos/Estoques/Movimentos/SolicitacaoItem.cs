using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstSolicitacaoItem")]
    public class EstoqueSolicitacaoItem : CamposPadraoCRUD
    {
        public long ProdutoId { get; set; }
        public long SolicitacaoId { get; set; }
        public decimal Quantidade { get; set; }
        public long EstadoSolicitacaoItemId { get; set; }
        public long ProdutoUnidadeId { get; set; }
        public decimal QuantidadeAtendida { get; set; }
        public long? EstoqueKitItemId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("SolicitacaoId")]
        public EstoquePreMovimento Solicitacao { get; set; }

        [ForeignKey("EstadoSolicitacaoItemId")]
        public EstoquePreMovimentoEstado EstadoSolicitacaoItem { get; set; }

        [ForeignKey("ProdutoUnidadeId")]
        public Unidade ProdutoUnidade { get; set; }

    }
}
