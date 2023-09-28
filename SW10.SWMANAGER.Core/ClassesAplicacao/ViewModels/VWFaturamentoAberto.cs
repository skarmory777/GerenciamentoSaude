using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("VWFaturamentoAberto")]
    public class VWFaturamentoAberto : Entity<long>
    {
        public string AnoMesVenc { get; set; }
        public string ValorDifEntregaGlosa { get; set; }
        public string ValorEntrega { get; set; }
        public string ValorQuitacaoAmbulatorio { get; set; }
        public string ValorQuitacaoInternacao { get; set; }
        public string AnoVenc { get; set; }
        public string MesVenc { get; set; }
        public string Empresa { get; set; }
        public string convenio { get; set; }
        public string QuantCredito { get; set; }
        public string ValorQuitacaoSemIdentificacao { get; set; }
        public string ValorQuitacaoLancamento { get; set; }
        public string ValorGlosaExterna { get; set; }
        public string ValorGlosaRecuperavel { get; set; }
        public string ValorGlosaIrrecuperavel { get; set; }
        public string ValorLancamentoAbertoSemGlosa { get; set; }
        public string ValorLancamentoAberto { get; set; }
    }
}