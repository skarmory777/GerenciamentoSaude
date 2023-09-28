using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Exporting
{
    public interface IListarFaturamentoEntregaLotesExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoEntregaLoteDto> KitsDtos);
    }
}
