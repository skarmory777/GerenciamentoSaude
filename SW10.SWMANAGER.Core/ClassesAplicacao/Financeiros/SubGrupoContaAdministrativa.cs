using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinSubGrupoContaAdministrativa")]
    public class SubGrupoContaAdministrativa : CamposPadraoCRUD
    {
        public long? GrupoContaAdministrativaId { get; set; }

        [ForeignKey("GrupoContaAdministrativaId")]
        public GrupoContaAdministrativa GrupoContaAdministrativa { get; set; }

        public bool IsSubGrupoContaNaoOperacional { get; set; }
        public bool IsUtilizadoCalculoSalario { get; set; }
        public bool IsSomandoDespesas { get; set; }
        public bool IsUsarFormula { get; set; }
        public bool IsNaoDetalharContaAdministrativa { get; set; }
    }
}
