using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    [Table("AssFormulaEstoqueItem")]
    public class FormulaEstoqueItem : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("Produto"), Column("EstProdutoId")]
        public long? ProdutoId { get; set; }

        [ForeignKey("Unidade"), Column("EstUnidadeId")]
        public long? UnidadeId { get; set; }

        public decimal Quantidade { get; set; }

        public bool IsVisivel { get; set; }

        public bool IsGeraSolicitacaoEstoque { get; set; }

        public Produto Produto { get; set; }

        public Unidade Unidade { get; set; }

        public string Descricao { get { return Produto.Descricao; } set { value = Produto.Descricao; } }

        [ForeignKey("FormulaEstoque"), Column("AssFormulaEstoqueId")]
        public long FormulaEstoqueId { get; set; }

        public FormulaEstoque FormulaEstoque { get; set; }

        public FormulaEstoqueItem()
        {
            Produto = new Produto();
        }
    }
}
