using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Faturamentos.EntregaContasRecebidas.Dto
{
    public class ConcilicarContasRecebidasInput
    {
        public long QuitacaoId { get; set; }        
        public DateTime DataConsolidado { get; set; }
        public float? ValorImposto { get; set; }
        public List<EntregaContaInput> EntregaContas { get; set; }
    }

    public class EntregaContaInput
    {
        public long EntregaContaId { get; set; }
        public long EntregaLoteId { get; set; }
        public float ValorRecebido { get; set; }
        public float? ValorGlosaRecuperavel { get; set; }
        public float? ValorGlosaIrrecuperavel { get; set; }
    }
}
