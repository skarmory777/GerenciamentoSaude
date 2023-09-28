using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposLogradouro
{
    [Table("SisTipoLogradouro")]
    public class TipoLogradouro : CamposPadraoCRUD
    {
        [StringLength(5)]
        public string Abreviacao { get; set; }

        public long? TabelaItemTissId { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominio TabelaDominio { get; set; }

    }
}
