using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasLaboratorios.Exporting
{
    public interface IListarBrasLaboratoriosExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoBrasLaboratorioDto> BrasLaboratoriosDtos);
    }
}
