using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("VWConsultaFaturamentoRecebimento")]
    public class VWConsultaFaturamentoRecebimento : Entity<long>
    {
        public string AnoMesPG { get; set; }
        public string ValorQuitacaoAmbulatorio { get; set; }
        public string ValorQuitacaoInternacao { get; set; }
        public string ValorQuitacaoSemIdentificacao { get; set; }
        public string ValorQuitacaoLancamento { get; set; }
    }
}