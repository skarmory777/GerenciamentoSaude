using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Ceps.Exporting
{
    public interface IListarCepExcelExporter
    {
        FileDto ExportToFile(List<CepDto> CepDtos);
    }
}
