using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposUnidade
{
    [Table("TipoUnidade")]
    public class TipoUnidade : CamposPadraoCRUD
    {
        [StringLength(30)]
        public string Nome { get; set; }
    }
}
