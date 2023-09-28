using System;

namespace SW10.SWMANAGER.Web.Areas.Mpa.Models.Aplicacao.Atendimentos.AgendamentoConsultas
{
    public class EventosViewModel
    {
        public long id { get; set; }
        public string title { get; set; }
        public bool allDay { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string color { get; set; }
    }
}