using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.CapitulosCID.Exporting
{
    public interface IListarCapitulosCIDExcelExporter
    {
        FileDto ExportToFile(List<CapituloCIDDto> list);
    }
}
