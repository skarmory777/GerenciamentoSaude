using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Exporting
{
    public interface IListarContasExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoContaDto> ContasDtos);
    }
}
