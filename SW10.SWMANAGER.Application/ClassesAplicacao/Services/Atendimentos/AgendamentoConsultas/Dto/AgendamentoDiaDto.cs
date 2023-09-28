namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.AgendamentoConsultas.Dto
{
    public class AgendamentoDiaDto
    {
        public string Sala { get; set; }
        public string Hora { get; set; }
        public string Medico { get; set; }
        public string Procedimento { get; set; }
        public string Paciente { get; set; }
        public long? EmpresaId { get; set; }
        public string Data { get; set; }
        public string Convenio { get; set; }
        public string Notas { get; set; }
    }
}
