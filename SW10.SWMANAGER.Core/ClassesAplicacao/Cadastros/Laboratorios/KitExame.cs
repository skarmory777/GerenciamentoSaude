using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabKitExame")]
    public class KitExame : CamposPadraoCRUD
    {
        public long? KitId { get; set; }
        public long? ExameId { get; set; }

        public bool IsLiberaKit { get; set; }

        [ForeignKey("KitId")]
        public Kit Kit { get; set; }

        public ICollection<KitExameItem> KitExameItens { get; set; }

        [ForeignKey("ExameId")]
        public Exame Exame { get; set; }

    }
}
