using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Pacotes.Dtos
{
    public class PacoteDto
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public long FaturamentoItemId { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Final { get; set; }
        public float? Quantidade { get; set; }
        public long? LocalUtilizacaoId { get; set; }
        public string LocalUtilizacaoDescricao { get; set; }
        public long? TurnoId { get; set; }
        public string turnoDescricao { get; set; }
        public DateTime? HoraInicio { get; set; }
        public DateTime? HoraFim { get; set; }

    }
}
