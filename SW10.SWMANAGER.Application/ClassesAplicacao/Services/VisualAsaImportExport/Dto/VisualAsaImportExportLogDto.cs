using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.VisualAsaImportExportLogs;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs.Dto
{
    [AutoMap(typeof(VisualAsaImportExportLog))]
    public class VisualAsaImportExportLogDto : CamposPadraoCRUDDto
    {
        public string Tabela { get; set; }
        public long IdAsa { get; set; }
        public long IdSw { get; set; }
        public string Operacao { get; set; }
    }
}
