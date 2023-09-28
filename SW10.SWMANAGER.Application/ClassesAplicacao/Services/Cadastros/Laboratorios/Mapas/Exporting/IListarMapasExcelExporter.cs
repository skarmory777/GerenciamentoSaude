using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Mapas.Exporting
{
    public interface IListarMapasExcelExporter
    {
        FileDto ExportToFile(List<MapaDto> MapasDtos);
    }
}
