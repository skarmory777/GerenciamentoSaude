using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Feriados.Exporting
{
    public interface IListarFeriadosExcelExporter
    {
        FileDto ExportToFile(List<FeriadoDto> list);
    }
}
