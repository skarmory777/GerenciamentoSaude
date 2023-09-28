namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Dto
{
    public class VisualizacaoContaRecebidaDto
    {
        public long Id { get; set;}
        public string Paciente { get; set;}
        public long QuitacaoId { get; set; }
        public long EntregaContaId { get; set; }
        public float ValorRecebido { get; set; }
        public float? ValorGlosaRecuperavel { get; set; }
        public float? ValorGlosaIrrecuperavel { get; set; }
    }
}
