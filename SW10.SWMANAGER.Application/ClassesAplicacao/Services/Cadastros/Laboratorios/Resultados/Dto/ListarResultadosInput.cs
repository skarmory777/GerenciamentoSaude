namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Laboratorios.Resultados.Dto
{
    public class ListarResultadosInput : ListarInput
    {
        public long? AtendimentoId { get; set; }
        public string TipoAtendimento { get; set; }

        public long? PacienteId { get; set; }
        public long? MedicoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? UnidadeId { get; set; }
    }
}
