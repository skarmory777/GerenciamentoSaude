using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CapitulosCID;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Categorias
{
    [Table("Categoria")]
    class Categoria
    {
        public long? CapituloCIDId { get; set; }
        [ForeignKey("CapituloCIDId")]
        public CapituloCID CapituloCid { get; set; }

        public long? GrupoCIDId { get; set; }
        [ForeignKey("GrupoCIDId")]
        public GrupoCID GrupoCid { get; set; }
    }
}
