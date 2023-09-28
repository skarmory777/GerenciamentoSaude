using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPalavrasChave
{
    [Table("ProdutoPalavraChave")]
    public class ProdutoPalavraChave : CamposPadraoCRUD
    {
        [StringLength(15)]
        public string Palavra { get; set; }

        [StringLength(255)]
        public string Observacao { get; set; }

    }
}
