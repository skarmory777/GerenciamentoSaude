using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Itens.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.TiposGrupo.Exporting
{
    public interface IListarFaturamentoTiposGrupoExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoItemDto> FaturamentoTiposGrupoDtos);
    }
}
