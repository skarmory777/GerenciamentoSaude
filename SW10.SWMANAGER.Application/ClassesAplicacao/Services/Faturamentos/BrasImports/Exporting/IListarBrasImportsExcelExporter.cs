using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasImports.Exporting
{
    public interface IListarBrasImportsExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoBrasImportDto> BrasImportsDtos);
    }
}
