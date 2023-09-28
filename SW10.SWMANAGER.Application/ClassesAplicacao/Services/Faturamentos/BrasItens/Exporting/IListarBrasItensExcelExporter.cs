using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasItens.Exporting
{
    public interface IListarBrasItensExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoBrasItemDto> BrasItensDtos);
    }
}
