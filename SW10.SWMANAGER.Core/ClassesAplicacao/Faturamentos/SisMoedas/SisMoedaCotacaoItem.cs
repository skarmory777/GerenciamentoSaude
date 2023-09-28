using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.SisMoedas
{
    [Table("SisMoedaCotacaoItem")]
    public class SisMoedaCotacaoItem : CamposPadraoCRUD
    {

        [ForeignKey("SisMoedaCotacaoId")]
        public SisMoedaCotacao SisMoedaCotacao { get; set; }
        public long? SisMoedaCotacaoId { get; set; }

        [ForeignKey("ItemId")]
        public FaturamentoItem Item { get; set; }
        public long? ItemId { get; set; }

    }
}

