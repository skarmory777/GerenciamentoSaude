using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Paises.Exporting
{
    public interface IListarPaisesExcelExporter
    {
        FileDto ExportToFile(List<PaisDto> list);
    }
}
