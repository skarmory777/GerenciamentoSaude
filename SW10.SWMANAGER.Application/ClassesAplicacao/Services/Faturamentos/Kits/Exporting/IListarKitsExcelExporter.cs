using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Kits.Exporting
{
    public interface IListarKitsExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoKitDto> KitsDtos);
    }
}
