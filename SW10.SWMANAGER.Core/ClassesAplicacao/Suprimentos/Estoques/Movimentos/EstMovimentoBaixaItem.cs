using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstMovimentoBaixaItem")]
    public class EstMovimentoBaixaItem : CamposPadraoCRUD
    {
        public long EstoqueMovimentoBaixaId { get; set; }
        public long EstoqueMovimentoItemId { get; set; }
        public decimal Quantidade { get; set; }

        [ForeignKey("EstoqueMovimentoBaixaId")]
        public EstoqueMovimento EstoqueMovimentoBaixa { get; set; }

        [ForeignKey("EstoqueMovimentoItemId")]
        public EstoqueMovimentoItem EstoqueMovimentoItem { get; set; }

    }
}
