using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosPortaria;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoRelacaoPortaria")]
    public class ProdutoRelacaoPortaria : CamposPadraoCRUD
    {
        public long ProdutoId { get; set; }
        public long ProdutoPortariaId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("ProdutoPortariaId")]
        public ProdutoPortaria ProdutoPortaria { get; set; }
    }
}