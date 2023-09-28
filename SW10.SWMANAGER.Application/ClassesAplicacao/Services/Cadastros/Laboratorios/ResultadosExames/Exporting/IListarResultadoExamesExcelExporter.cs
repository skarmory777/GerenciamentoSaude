using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.ResultadosExames.Exporting
{
    public interface IListarResultadoExamesExcelExporter
    {
        FileDto ExportToFile(List<ResultadoExameIndexDto> ResultadoExamesDtos);
    }
}
