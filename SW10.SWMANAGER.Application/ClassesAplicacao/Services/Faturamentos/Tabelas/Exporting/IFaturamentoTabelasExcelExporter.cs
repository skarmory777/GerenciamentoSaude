using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Tabelas.Exporting
{
    public interface IListarFaturamentoTabelasExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoTabelaDto> FaturamentoTabelasDtos);
    }
}
