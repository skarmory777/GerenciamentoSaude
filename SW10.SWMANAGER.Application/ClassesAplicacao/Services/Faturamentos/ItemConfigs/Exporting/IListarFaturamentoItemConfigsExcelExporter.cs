using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItemConfigs.Exporting
{
    public interface IListarItemConfigsExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoItemConfigDto> ItemConfigsDtos);
    }
}
