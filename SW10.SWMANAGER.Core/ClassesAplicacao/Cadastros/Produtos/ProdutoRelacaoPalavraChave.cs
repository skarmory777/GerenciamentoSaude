using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPalavrasChave;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoRelacaoPalavraChave")]
    public class ProdutoRelacaoPalavraChave : CamposPadraoCRUD
    {
        public long ProdutoId { get; set; }
        public long ProdutoPalavraChaveId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("ProdutoPalavraChaveId")]
        public ProdutoPalavraChave ProdutoPalavraChave { get; set; }
    }
}
