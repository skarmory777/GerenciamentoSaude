namespace SW10.SWMANAGER.ClassesAplicacao.Services.Atendimentos.Atendimentos.Dto
{
    public class PacienteMedicoDto
    {
        public long? PacienteId { get; set; }
        public string PacienteNome { get; set; }
        public long? MedicoId { get; set; }
        public string MedicoNome { get; set; }

        public long? UnidadeOrganizacionalId { get; set; }

        public string UnidadeOrganizacional { get; set; }


    }
}
