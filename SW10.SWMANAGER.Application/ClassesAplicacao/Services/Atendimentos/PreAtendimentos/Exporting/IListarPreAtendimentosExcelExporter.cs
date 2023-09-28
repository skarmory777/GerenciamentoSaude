using SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.PreAtendimentos.Exporting
{
    public interface IListarPreAtendimentosExcelExporter
    {
        FileDto ExportToFile(List<PreAtendimentoDto> preAtendimentosDto);
    }
}
