using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoListaSubstituicao")]
    public class ProdutoListaSubstituicao : CamposPadraoCRUD
    {

        public long ProdutoId { get; set; }
        public long ProdutoSubstituicaoId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }
    }
}
