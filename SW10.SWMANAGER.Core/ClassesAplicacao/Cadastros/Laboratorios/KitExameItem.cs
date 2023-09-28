using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabKitExameItem")]
    public class KitExameItem : CamposPadraoCRUD
    {
        public long KitExameId { get; set; }
        public long FaturamentoItemId { get; set; }
        public bool IsLiberaKit { get; set; }

        [ForeignKey("KitExameId")]
        public KitExame KitExame { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }
    }
}
