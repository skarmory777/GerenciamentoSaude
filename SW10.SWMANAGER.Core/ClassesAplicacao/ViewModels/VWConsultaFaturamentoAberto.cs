using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("VWConsultaFaturamentoAberto")]
    public class VWConsultaFaturamentoAberto : Entity<long>
    {
        public string AnoMesVenc { get; set; }
        public string ValorDifEntregaGlosa { get; set; }
        public string ValorEntrega { get; set; }
        public string ValorQuitacaoAmbulatorio { get; set; }
        public string ValorQuitacaoInternacao { get; set; }
        public string ValorLancamentoAberto { get; set; }
        public string ValorGlosa { get; set; }

    }
}