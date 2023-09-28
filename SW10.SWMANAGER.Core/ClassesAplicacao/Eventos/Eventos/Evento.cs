using SW10.SWMANAGER.ClassesAplicacao.Eventos.EventosGrupos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Eventos.Eventos
{
    [Table("QuaEvento")]
    public class Evento : CamposPadraoCRUD
    {
        public bool RealizaSindicancia { get; set; }

        [ForeignKey("EventoGrupo"), Column("QuaEventoGrupoId")]
        public long? EventoGrupoId { get; set; }
        public EventoGrupo EventoGrupo { get; set; }
    }
}
