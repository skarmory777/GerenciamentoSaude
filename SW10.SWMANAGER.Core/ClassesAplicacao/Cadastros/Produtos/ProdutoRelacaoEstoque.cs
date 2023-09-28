using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosEstoque;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLocalizacao;
using System.ComponentModel.DataAnnotations.Schema;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoRelacaoEstoque")]
    public class ProdutoRelacaoEstoque : CamposPadraoCRUD
    {

            public long ProdutoId { get; set; }
            public long ProdutoEstoqueId { get; set; }
            public long ProdutoLocalizacaoId { get; set; }

            public long EstoqueMinimo { get; set; }
            public long EstoqueMaximo { get; set; }
            public long PontoPedido { get; set; }

            [ForeignKey("ProdutoId")]
            public virtual Produto Produto { get; set; }

            [ForeignKey("ProdutoLocalizacaoId")]
            public virtual ProdutoLocalizacao ProdutoLocalizacao { get; set; }

            [ForeignKey("ProdutoEstoqueId")]
            public virtual ProdutoEstoque ProdutoEstoque { get; set; }
    }
}
