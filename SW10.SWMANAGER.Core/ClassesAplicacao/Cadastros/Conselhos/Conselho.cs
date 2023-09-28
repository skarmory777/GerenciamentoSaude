using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TabelasDominio;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Conselhos
{
    [Table("SisConselho")]
    public class Conselho : CamposPadraoCRUD
    {
        [StringLength(10)]
        public string Sigla { get; set; }

        [StringLength(5)]
        public string Uf { get; set; }

        [StringLength(75)]
        public string NomeEstado { get; set; }

        public long? TabelaItemTissId { get; set; }

        [ForeignKey("TabelaItemTissId")]
        public virtual TabelaDominio TabelaDominio { get; set; }

    }
}
