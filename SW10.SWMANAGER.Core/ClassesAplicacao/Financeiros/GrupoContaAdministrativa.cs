using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinGrupoContaAdministrativa")]
    public class GrupoContaAdministrativa : CamposPadraoCRUD
    {
        public bool IsValorIncideResultadoOperacionalEmpresa { get; set; }

        public long? GrupoDREId { get; set; }

        [ForeignKey("GrupoDREId")]
        public GrupoDRE GrupoDRE { get; set; }

        public IList<SubGrupoContaAdministrativa> SubGruposContasAdministrativa { get; set; }
    }
}
