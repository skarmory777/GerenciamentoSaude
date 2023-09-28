using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Assistenciais.Exporting
{
    public interface IListarAssistencialAtendimentosExcelExporter
    {
        FileDto ExportToFile(List<AtendimentoIndexDto> AtendimentosDto);
    }
}
