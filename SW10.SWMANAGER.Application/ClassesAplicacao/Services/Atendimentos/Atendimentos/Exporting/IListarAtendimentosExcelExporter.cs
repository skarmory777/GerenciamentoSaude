using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Exporting
{
    public interface IListarAtendimentosExcelExporter
    {
        FileDto ExportToFile(List<AtendimentoDto> agendamentoConsultasDto);
    }
}
