using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Exporting
{
    public interface IListarConveniosExcelExporter
    {
        FileDto ExportToFile(List<ConvenioDto> profissoesDtos);
    }
}
