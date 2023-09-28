using SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Exporting
{
    public interface IListarAgendamentoConsultasExcelExporter
    {
        FileDto ExportToFile(List<AgendamentoConsultaDto> agendamentoConsultasDto);
    }
}
