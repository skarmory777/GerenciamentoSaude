using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos
{
    [Table("SisCbo")]
    public class Cbo : CamposPadraoCRUD
    {
        [ForeignKey("CboFamilia"), Column("SisCboFamiliaId")]
        public long? CboFamiliaId { get; set; }
        public CboFamilia CboFamilia { get; set; }

        [ForeignKey("CboTipo"), Column("SisCboTipoId")]
        public long? CboTipoId { get; set; }
        public CboTipo CboTipo { get; set; }

        public long? TabelaItemTissId { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominio TabelaDominio { get; set; }
    }
}
