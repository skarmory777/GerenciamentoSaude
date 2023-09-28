using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Faturamentos.Guias.Exporting
{
    public interface IListarGuiasExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoGuiaDto> list);
    }
}
