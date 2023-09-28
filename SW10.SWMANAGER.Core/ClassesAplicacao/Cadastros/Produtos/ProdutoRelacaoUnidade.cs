using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosTiposUnidade;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoRelacaoUnidade")]
    public class ProdutoRelacaoUnidade : CamposPadraoCRUD
    {
        public bool isAtivo { get; set; }

        public bool Prescricao { get; set; }

        public long ProdutoId { get; set; }

        public long UnidadeId { get; set; }

        public long TipoUnidadeId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("UnidadeId")]
        public ProdutoUnidade ProdutoUnidade { get; set; }

        [ForeignKey("TipoUnidadeId")]
        public ProdutoTipoUnidade TipoUnidade { get; set; }

    }
}
