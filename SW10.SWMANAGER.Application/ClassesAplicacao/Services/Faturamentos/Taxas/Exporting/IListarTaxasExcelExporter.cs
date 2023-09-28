using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Taxas.Exporting
{
    public interface IListarTaxasExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoTaxaDto> TaxasDtos);
    }
}
