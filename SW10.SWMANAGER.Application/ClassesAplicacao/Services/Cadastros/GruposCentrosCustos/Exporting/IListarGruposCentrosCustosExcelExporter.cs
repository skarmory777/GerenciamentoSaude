using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.GruposCentrosCustos.Exporting
{
    public interface IListarGruposCentrosCustosExcelExporter
    {
        FileDto ExportToFile(List<GrupoCentroCustoDto> GruposCentrosCustosDtos);
    }
}
