using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.TabelasResultados.Exporting
{
    public interface IListarTabelaResultadosExcelExporter
    {
        FileDto ExportToFile(List<TabelaResultadoDto> TabelaResultadosDtos);
    }
}
