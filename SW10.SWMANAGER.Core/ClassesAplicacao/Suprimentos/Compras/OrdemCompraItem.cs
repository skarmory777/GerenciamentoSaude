using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Compras
{
    [Table("CmpOrdemCompraItem")]
    public class OrdemCompraItem : CamposPadraoCRUD
    {
        [ForeignKey("OrdemCompra"), Column("CmpOrdemCompraId")]
        public long OrdemCompraId { get; set; }
        public CmpOrdemCompra OrdemCompra { get; set; }
 
        [ForeignKey("RequisicaoItem"), Column("CmpRequisicaoItemId")]
        public long? RequisicaoItemId { get; set; }
        public CompraRequisicaoItem RequisicaoItem { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal Quantidade { get; set; }

        public decimal ValorTotal { get; set; }
        
        [ForeignKey("Unidade"), Column("UnidadeId")]
        public long UnidadeId { get; set; }
        public Unidade Unidade { get; set; }

        [ForeignKey("Produto"), Column("EstProdutoId")]
        public long ProdutoId { get; set; }
        public Produto Produto { get; set; }
    }
}