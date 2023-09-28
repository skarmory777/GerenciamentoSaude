using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Exporting
{
    public interface IListarResultadosExcelExporter
    {
        FileDto ExportToFile(List<ResultadoIndexDto> ResultadosDtos);
    }
}
