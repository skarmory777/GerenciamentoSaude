using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.ItensTabela.Exporting
{
    public interface IListarFaturamentoItensTabelaExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoItemTabelaDto> ItensTabelaDtos);
    }
}
