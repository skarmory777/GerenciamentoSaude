using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens
{
    [Table("FatItemConfig")]
    public class FaturamentoItemConfig : CamposPadraoCRUD
    {
        [ForeignKey("Convenio"), Column("ConvenioId")]
        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        [ForeignKey("Plano"), Column("PlanoId")]
        public long? PlanoId { get; set; }
        public Plano Plano { get; set; }

        [ForeignKey("Item"), Column("ItemId")]
        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }


        [ForeignKey("ItemCobrar"), Column("ItemCobrarId")]
        public long? ItemCobrarId { get; set; }
        public FaturamentoItem ItemCobrar { get; set; }
    }

}


