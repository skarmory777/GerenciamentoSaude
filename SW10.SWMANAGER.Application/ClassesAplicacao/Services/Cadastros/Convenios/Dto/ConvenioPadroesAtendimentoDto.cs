namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Convenios.Dto
{
    public class ConvenioPadroesAtendimentoDto
    {
        public long? EmpresaPadraoEmergenciaId { get; set; }
        public string EmpresaPadraoEmergencia { get; set; }
        public long? MedicoPadraoEmergenciaId { get; set; }
        public string MedicoPadraoEmergencia { get; set; }
        public long? EspecialidadePadraoEmergenciaId { get; set; }
        public string EspecialidadePadraoEmergencia { get; set; }
        public long? EmpresaPadraoInternacaoId { get; set; }
        public string EmpresaPadraoInternacao { get; set; }
        public long? MedicoPadraoInternacaoId { get; set; }
        public string MedicoPadraoInternacao { get; set; }
        public long? EspecialidadePadraoInternacaoId { get; set; }
        public string EspecialidadePadraoInternacao { get; set; }
        public bool IsParticular { get; set; }
    }
}
