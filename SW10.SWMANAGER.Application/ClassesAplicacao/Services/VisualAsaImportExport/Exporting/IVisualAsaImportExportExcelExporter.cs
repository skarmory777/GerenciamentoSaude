using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.VisualAsaImportExportLogs.Exporting
{
    public interface IListarVisualAsaImportExportLogExcelExporter
    {
        FileDto ExportToFile(List<VisualAsaImportExportLogDto> visualAsaImportExportLogDtos);
    }
}
