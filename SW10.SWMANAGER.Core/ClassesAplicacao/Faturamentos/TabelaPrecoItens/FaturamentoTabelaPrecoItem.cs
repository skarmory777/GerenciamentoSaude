using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TabelaPrecoItens
{

    [Table("FatTabelaPrecoItem")]
    public class FaturamentoTabelaPrecoItem : CamposPadraoCRUD
    {
        //[ForeignKey("TabelaPrecoId")]
        //public virtual FaturamentoTabelaPreco TabelaPreco { get; set; }
        //public long TabelaPrecoId { get; set; }

        [ForeignKey("ItemId")]
        public FaturamentoItem Item { get; set; }
        public long ItemId { get; set; }
    }

}


