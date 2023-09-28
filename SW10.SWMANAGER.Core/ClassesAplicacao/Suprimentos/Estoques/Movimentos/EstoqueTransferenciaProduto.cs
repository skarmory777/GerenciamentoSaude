using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoqueTransferenciaProduto")]
    public class EstoqueTransferenciaProduto : CamposPadraoCRUD
    {
        public long PreMovimentoEntradaId { get; set; }
        public long PreMovimentoSaidaId { get; set; }

        [ForeignKey("PreMovimentoEntradaId")]
        public EstoquePreMovimento PreMovimentoEntrada { get; set; }

        [ForeignKey("PreMovimentoSaidaId")]
        public EstoquePreMovimento PreMovimentoSaida { get; set; }
    }
}
