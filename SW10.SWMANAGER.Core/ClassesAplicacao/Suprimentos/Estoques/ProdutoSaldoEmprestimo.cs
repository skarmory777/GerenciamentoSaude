using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques
{
    [Table("ProdutoSaldoEmprestimo")]
    public class ProdutoSaldoEmprestimo : CamposPadraoCRUD
    {
        public long ProdutoId { get; set; }
        public long FornecedorId { get; set; }
        public decimal QuantidadeAtual { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("FornecedorId")]
        public Fornecedor Fornecedor { get; set; }
    }
}
