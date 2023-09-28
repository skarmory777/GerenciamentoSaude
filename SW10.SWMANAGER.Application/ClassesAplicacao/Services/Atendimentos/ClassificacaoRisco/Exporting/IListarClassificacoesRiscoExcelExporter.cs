using SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ClassificacoesRisco.Exporting
{
    public interface IListarClassificacoesRiscoExcelExporter
    {
        FileDto ExportToFile(List<ClassificacaoRiscoDto> preAtendimentosDto);
    }
}
