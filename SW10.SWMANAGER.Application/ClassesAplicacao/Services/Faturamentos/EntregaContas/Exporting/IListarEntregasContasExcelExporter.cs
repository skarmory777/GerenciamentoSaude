using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Entregas.Exporting
{
    public interface IListarFaturamentoEntregaContasExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoEntregaContaDto> KitsDtos);
    }
}
