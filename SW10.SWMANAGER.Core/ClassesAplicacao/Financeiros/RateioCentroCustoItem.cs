using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinRateioCentroCustoItem")]
    public class RateioCentroCustoItem : CamposPadraoCRUD
    {
        public long? RateioCentroCustoId { get; set; }

        [ForeignKey("RateioCentroCustoId")]
        public RateioCentroCusto RateioCentroCusto { get; set; }

        public decimal PercentualRateio { get; set; }
        public long CentroCustoId { get; set; }

        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }
    }
}
