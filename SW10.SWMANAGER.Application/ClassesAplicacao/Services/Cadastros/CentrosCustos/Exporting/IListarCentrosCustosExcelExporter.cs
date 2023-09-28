using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CentrosCustos.Exporting
{
    public interface IListarCentrosCustosExcelExporter
    {
        FileDto ExportToFile(List<CentroCustoDto> CentrosCustosDtos);
    }
}
