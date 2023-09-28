using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabFormataItem")]
    public class FormataItem : CamposPadraoCRUD
    {
        public long? FormataId { get; set; }
        public long? ItemResultadoId { get; set; }


        public int Ordem { get; set; }
        public int? OrdemRegistro { get; set; }
        public string Formula { get; set; }

        public bool IsBI { get; set; }

        public bool IsRefExame { get; set; }

        [ForeignKey("FormataId")]
        public Formata Formata { get; set; }

        [ForeignKey("ItemResultadoId")]
        public ItemResultado ItemResultado { get; set; }
    }
}