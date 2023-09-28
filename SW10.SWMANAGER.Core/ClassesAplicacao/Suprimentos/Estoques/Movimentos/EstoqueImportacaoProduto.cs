using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos
{
    [Table("EstImportacaoProduto")]
    public class EstoqueImportacaoProduto : CamposPadraoCRUD
    {
       public long? FornecedorId { get; set; }
        public long ProdutoId { get; set; }
        public string CNPJNota { get; set; }
        public string CodigoProdutoNota { get; set; }
        public long UnidadeId { get; set; }
        public decimal Fator { get; set; }

        ////[ForeignKey("FornecedorId")]
        ////public SisFornecedor Fornecedor { get; set; }

        [ForeignKey("ProdutoId")]
        public Produto Produto { get; set; }

        [ForeignKey("UnidadeId")]
        public Unidade Unidade { get; set; }


    }
}
