using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.VisualAsaImportExportLogs
{
    [Table("SisVisualAsaImportExportLog")]
    public class VisualAsaImportExportLog : CamposPadraoCRUD
    {
        public string Tabela { get; set; }
        public long IdAsa { get; set; }
        public long IdSw { get; set; }
        public string Operacao { get; set; }
    }
}
