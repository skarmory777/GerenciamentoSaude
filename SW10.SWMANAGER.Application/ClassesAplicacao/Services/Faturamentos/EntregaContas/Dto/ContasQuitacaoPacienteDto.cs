using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContas.Dto
{
    public class ContasQuitacaoPacienteDto
    {
        public long Id { get; set; }
        public long? EntregaLoteId { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public string Matricula { get; set; }
        public string TipoGuia { get; set; }
        public string NumeroGuia { get; set; }
        public string PacienteNomeCompleto { get; set; }
        public string ConvenioNomeFantasia { get; set; }
        public float ValorEntregue { get; set; }
        public float GlosaRecuperavel { get; set; }
        public float GlosaIrrecuperavel { get; set; }
        public float ValorRecebidoAnterior { get; set; }
        public float ValorRecebidoAtual { get; set; }
    }
}
