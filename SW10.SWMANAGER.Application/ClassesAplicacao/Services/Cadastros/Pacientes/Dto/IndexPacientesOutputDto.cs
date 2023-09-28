namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Pacientes.Dto
{
    public class IndexPacientesOutputDto : CamposPadraoCRUDDto
    {
        public long atendId { get; set; }
        public int CodigoPaciente { get; set; }
        public string NomeCompleto { get; set; }
        public long? PacienteId { get; set; }
    }
}
