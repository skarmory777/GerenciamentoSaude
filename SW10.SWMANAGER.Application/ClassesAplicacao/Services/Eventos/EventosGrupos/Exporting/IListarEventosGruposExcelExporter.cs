using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos.Dto;
using SW10.SWMANAGER.Dto;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos.Exporting
{
    public interface IListarEventosGruposExcelExporter
    {
        FileDto ExportToFile(List<EventoGrupoDto> agendamentoConsultasDto);
    }
}
