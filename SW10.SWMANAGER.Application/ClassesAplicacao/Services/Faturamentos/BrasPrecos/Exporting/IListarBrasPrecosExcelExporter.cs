using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.BrasPrecos.Exporting
{
    public interface IListarBrasPrecosExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoBrasPrecoDto> BrasPrecosDtos);
    }
}
