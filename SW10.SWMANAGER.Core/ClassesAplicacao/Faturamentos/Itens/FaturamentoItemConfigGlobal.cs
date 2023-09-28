using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens
{
    [Table("FatItemConfigGlobal")]
    public class FaturamentoItemConfigGlobal : CamposPadraoCRUD
    {
        [ForeignKey("Global"), Column("GlobalId")]
        public long? GlobalId { get; set; }
        public FaturamentoGlobal Global { get; set; }

        [ForeignKey("Item"), Column("ItemId")]
        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }

        [ForeignKey("ItemCobrar"), Column("ItemCobrarId")]
        public long? ItemCobrarId { get; set; }
        public FaturamentoItem ItemCobrar { get; set; }
    }

}


