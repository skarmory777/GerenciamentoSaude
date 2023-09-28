using SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Visitantes.Exporting
{
    public interface IListarVisitantesExcelExporter
    {
        FileDto ExportToFile(List<VisitanteDto> visitanteDto);
    }
}
