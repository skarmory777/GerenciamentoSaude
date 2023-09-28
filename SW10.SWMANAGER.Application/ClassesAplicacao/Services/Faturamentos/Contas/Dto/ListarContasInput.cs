namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.Contas.Dto
{
    public class ListarContasInput : ListarInput // PagedAndSortedInputDto, IShouldNormalize
    {
        public string PacienteId { get; set; }
        public string ConvenioId { get; set; }
        public string AtendimentoId { get; set; }
        public string GuiaId { get; set; }
        public string MedicoId { get; set; }
        public string NumeroGuia { get; set; }
        public bool IsEmergencia { get; set; }
        public bool IsInternacao { get; set; }
        public bool ApenasConferidas { get; set; }
        public bool IsLaboratorio { get; set; }
        public bool IsImagem { get; set; }

        public string UsuarioId { get; set; }

        public bool IgnoraData { get; set; }

        public string Periodo { get; set; }
    }
}
