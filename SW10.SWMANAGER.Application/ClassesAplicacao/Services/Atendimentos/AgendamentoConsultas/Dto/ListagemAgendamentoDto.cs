using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class ListagemAgendamentoDto
    {
        public long Id { get; set; }
        public string Paciente { get; set; }

        public long? PacienteId { get; set; }
        public string Medico { get; set; }

        public long? MedicoId { get; set; }
        public DateTime Data { get; set; }
        public DateTime Hora { get; set; }
        public string Sala { get; set; }

        public long? SalaId { get; set; }
        public string Procedimento { get; set; }
        public string TipoCirurgia { get; set; }

        public long? TipoCirurgiaId { get; set; }
        public string Status { get; set; }
        public long StatusId { get; set; }
    }
}
