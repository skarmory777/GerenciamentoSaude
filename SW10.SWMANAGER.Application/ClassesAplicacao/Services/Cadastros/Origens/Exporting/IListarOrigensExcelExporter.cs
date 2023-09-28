using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Origens.Exporting
{
    public interface IListarOrigensExcelExporter
    {
        FileDto ExportToFile(List<OrigemDto> OrigensDtos);
    }
}
