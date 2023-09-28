using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ConfigConvenios.Exporting
{
    public interface IListarConfigConveniosExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoConfigConvenioDto> ConfigConveniosDtos);
    }
}
