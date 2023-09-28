using Abp.AutoMapper;

using SW10.SWMANAGER.ClassesAplicacao.Eventos.EventosGrupos;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Eventos.EventosGrupos.Dto
{
    [AutoMap(typeof(EventoGrupo))]
    public class EventoGrupoDto : CamposPadraoCRUD
    {

    }
}
