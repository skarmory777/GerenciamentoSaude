using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinContaAdministrativa")]
    public class ContaAdministrativa : CamposPadraoCRUD
    {
        public bool IsNaoContabilizarPagarGerencial { get; set; }
        public bool IsNaoContabilizarReceberGerencial { get; set; }
        public bool IsReceita { get; set; }
        public bool IsDespesa { get; set; }

        public long? RateioCentroCustoId { get; set; }

        [ForeignKey("RateioCentroCustoId")]
        public RateioCentroCusto RateioCentroCusto { get; set; }

        public long SubGrupoContaAdministrativaId { get; set; }

        [ForeignKey("SubGrupoContaAdministrativaId")]
        public SubGrupoContaAdministrativa SubGrupoContaAdministrativa { get; set; }

        public List<ContaAdministrativaCentroCusto> ContaAdministrativaCentrosCustos { get; set; }
        public List<ContaAdministrativaEmpresa> ContaAdministrativaEmpresas { get; set; }
    }
}
