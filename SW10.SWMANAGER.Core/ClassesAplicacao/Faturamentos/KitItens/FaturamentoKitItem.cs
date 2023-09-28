using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits
{
    [Table("FatKitItem")]
    public class FaturamentoKitItem : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(255)]
        public override string Descricao { get; set; }

        [ForeignKey("FatKit"), Column("FatKitId")]
        public long? FatKitId { get; set; }
        public FaturamentoKit FatKit { get; set; }

        [ForeignKey("FatItem"), Column("FatItemId")]
        public long? FatItemId { get; set; }
        public FaturamentoItem FatItem { get; set; }

        public decimal Quantidade { get; set; }
    }

}


