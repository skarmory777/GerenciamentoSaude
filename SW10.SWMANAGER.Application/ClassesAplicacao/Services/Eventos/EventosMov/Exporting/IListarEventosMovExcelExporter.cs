using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosMov.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosMov.Exporting
{
    public interface IListarEventosMovExcelExporter
    {
        FileDto ExportToFile(List<EventoMovDto> agendamentoConsultasDto);
    }
}
