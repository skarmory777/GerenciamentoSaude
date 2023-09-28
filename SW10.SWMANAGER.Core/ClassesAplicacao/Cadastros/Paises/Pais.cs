using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Paises
{
    [Table("SisPais")]
    public class Pais : CamposPadraoCRUD, IDescricao
    {
        [MaxLength(60)]
        public string Nome { get; set; }
        [MaxLength(10)]
        public string Sigla { get; set; }
        //public virtual ICollection<Estado> Estados { get; set; }
    }
}
