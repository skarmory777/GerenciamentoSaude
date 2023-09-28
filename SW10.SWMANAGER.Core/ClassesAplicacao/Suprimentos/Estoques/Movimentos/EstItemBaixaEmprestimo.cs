using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstBaixaEmprestimoItem")]
    public class EstoqueBaixaEmprestimoItem : CamposPadraoCRUD
    {
        [Column("EstBaixaEmprestimoId")]
        public long EstoqueBaixaEmprestimoId { get; set; }

        [Column("EstBaixaMovimentoItemEntradaId")]
        public long EstoqueBaixaMovimentoItemEntradaId { get; set; }

        [Column("EstBaixaMovimentoItemSaidaId")]
        public long EstoqueBaixaMovimentoItemSaidaId { get; set; }

        public decimal QuantidadeBaixa { get; set; }

        [ForeignKey("EstoqueBaixaMovimentoItemEntradaId")]
        public EstoqueMovimentoItem EstoqueMovimentoItemBaixaEntrada { get; set; }

        [ForeignKey("EstoqueBaixaMovimentoItemSaidaId")]
        public EstoqueMovimentoItem EstoqueMovimentoItemBaixaSaida { get; set; }

        [ForeignKey("EstoqueBaixaEmprestimoId")]
        public EstoqueBaixaEmprestimo EstoqueBaixaEmprestimo { get; set; }



    }
}
