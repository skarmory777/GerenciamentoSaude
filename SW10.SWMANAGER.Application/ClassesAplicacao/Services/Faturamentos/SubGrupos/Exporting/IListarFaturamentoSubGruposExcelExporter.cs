using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.SubGrupos.Exporting
{
    public interface IListarFaturamentoSubGruposExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoSubGrupoDto> SubGruposDtos);
    }
}
