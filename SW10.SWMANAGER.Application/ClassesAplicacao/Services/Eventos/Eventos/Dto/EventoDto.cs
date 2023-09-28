using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Eventos.Eventos;
using SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.Eventos.Dto
{
    [AutoMap(typeof(Evento))]
    public class EventoDto : CamposPadraoCRUD
    {
        public bool RealizaSindicancia { get; set; }

        public long? EventoGrupoId { get; set; }
        public EventoGrupoDto EventoGrupo { get; set; }
    }
}
