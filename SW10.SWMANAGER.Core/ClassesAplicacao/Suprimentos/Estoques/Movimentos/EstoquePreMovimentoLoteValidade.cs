using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstoquePreMovimentoLoteValidade")]
    public class EstoquePreMovimentoLoteValidade : CamposPadraoCRUD
    {

        public long EstoquePreMovimentoItemId { get; set; }

        public long LoteValidadeId { get; set; }

        public decimal Quantidade { get; set; }

        [ForeignKey("EstoquePreMovimentoItemId")]
        public EstoquePreMovimentoItem EstoquePreMovimentoItem { get; set; }

        [ForeignKey("LoteValidadeId")]
        public LoteValidade LoteValidade { get; set; }

    }
}
