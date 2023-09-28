using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Desenvolvimento
{
    [AutoMap(typeof(TarefaIntervalo))]
    public class TarefaIntervaloDto : CamposPadraoCRUDDto
    {
        public override string Codigo { get; set; }
        public override string Descricao { get; set; }
        public long? ResponsavelId { get; set; }

        public string ResponsavelNome { get; set; }
        public long TarefaId { get; set; }
        public /*virtual*/ Tarefa Tarefa { get; set; }

        public DateTime Inicio { get; set; }
        public DateTime? Fim { get; set; }

        public string TempoDecorrido { get; set; }

        public string CalcularTempoDecorrido()
        {
            TimeSpan timeSpan = ((DateTime)Fim - (DateTime)Inicio);
            TempoDecorrido = string.Format("{0} horas, {1} minutos", timeSpan.Hours, timeSpan.Minutes);
            return TempoDecorrido;
        }
    }
}
