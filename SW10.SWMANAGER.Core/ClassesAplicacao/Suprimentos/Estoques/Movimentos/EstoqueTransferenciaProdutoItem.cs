using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoqueTransferenciaProdutoItem")]
    public class EstoqueTransferenciaProdutoItem : CamposPadraoCRUD
    {
        public long PreMovimentoEntradaItemId { get; set; }
        public long PreMovimentoSaidaItemId { get; set; }
        public long EstoqueTransferenciaProdutoID { get; set; }

        [ForeignKey("PreMovimentoEntradaItemId")]
        public EstoquePreMovimentoItem PreMovimentoEntradaItem { get; set; }

        [ForeignKey("PreMovimentoSaidaItemId")]
        public EstoquePreMovimentoItem PreMovimentoSaidaItem { get; set; }

        [ForeignKey("EstoqueTransferenciaProdutoID")]
        public EstoqueTransferenciaProduto EstoqueTransferenciaProduto { get; set; }
    }
}
