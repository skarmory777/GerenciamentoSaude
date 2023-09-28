using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao
{
    [Table("SisTipoAcomodacao")]
    public class TipoAcomodacao : CamposPadraoCRUD
    {
        [StringLength(10)]
        public string CodigoTiss { get; set; }

        public long? TabelaItemTissId { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominio TabelaDominio { get; set; }
    }
}
