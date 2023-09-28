using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Intervalos.Exporting
{
    public interface IListarIntervalosExcelExporter
    {
        FileDto ExportToFile(List<IntervaloDto> intervalosDtos);
    }
}
