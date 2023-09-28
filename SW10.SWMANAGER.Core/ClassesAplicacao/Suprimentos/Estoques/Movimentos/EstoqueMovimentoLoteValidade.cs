using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoqueMovimentoLoteValidade")]
    public class EstoqueMovimentoLoteValidade : CamposPadraoCRUD
    {

        public long EstoqueMovimentoItemId { get; set; }

        public long LoteValidadeId { get; set; }

        public decimal Quantidade { get; set; }

        [ForeignKey("EstoqueMovimentoItemId")]
        public EstoqueMovimentoItem EstoqueMovimentoItem { get; set; }

        [ForeignKey("LoteValidadeId")]
        public LoteValidade LoteValidade { get; set; }

    }
}
