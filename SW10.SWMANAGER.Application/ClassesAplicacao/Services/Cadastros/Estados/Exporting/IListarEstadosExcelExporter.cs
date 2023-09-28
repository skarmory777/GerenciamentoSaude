using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Estados.Exporting
{
    public interface IListarEstadosExcelExporter
    {
        FileDto ExportToFile(List<EstadoDto> list);
    }
}
