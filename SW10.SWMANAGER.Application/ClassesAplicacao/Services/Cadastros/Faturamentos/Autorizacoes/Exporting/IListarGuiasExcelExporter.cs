using System.Collections.Generic;
using SW10.SWMANAGER.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Exporting
{
    public interface IListarGuiasExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoGuiaDto> list);
    }
}
