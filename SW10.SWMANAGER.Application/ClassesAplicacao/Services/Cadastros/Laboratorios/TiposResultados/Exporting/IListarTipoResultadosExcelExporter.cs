using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TiposResultados.Exporting
{
    public interface IListarTipoResultadosExcelExporter
    {
        FileDto ExportToFile(List<TipoResultadoDto> TipoResultadosDtos);
    }
}
