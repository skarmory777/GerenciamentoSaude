using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Produtos
{
    [Table("ProdutoRelacaoLaboratorio")]
    public class ProdutoRelacaoLaboratorio : CamposPadraoCRUD
    {

        public long ProdutoId { get; set; }
        public long ProdutoLaboratorioId { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("ProdutoLaboratorioId")]
        public SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio.EstoqueLaboratorio ProdutoLaboratorio { get; set; }
    }
}
