using SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Grupos.Exporting
{
    public interface IListarFaturamentoGruposExcelExporter
    {
        FileDto ExportToFile(List<FaturamentoGrupoDto> GruposDtos);
    }
}
