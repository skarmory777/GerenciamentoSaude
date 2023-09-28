using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinContaAdministrativaCentroCusto")]
    public class ContaAdministrativaCentroCusto : CamposPadraoCRUD
    {
        public long? ContaAdministrativaId { get; set; }

        [ForeignKey("ContaAdministrativaId")]
        public ContaAdministrativa ContaAdministrativa { get; set; }

        public long CentroCustoId { get; set; }

        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }

        public decimal Percentual { get; set; }
    }
}
