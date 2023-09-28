using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens
{
    [Table("AssFormulaEstoque")]
    public class FormulaEstoque : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("EstoqueOrigem"), Column("EstoqueOrigemId")]
        public long? EstoqueId { get; set; }

        public Estoque EstoqueOrigem { get; set; }

        [ForeignKey("Produto"), Column("EstProdutoId")]
        public long? ProdutoId { get; set; }

        public Produto Produto { get; set; }

        [ForeignKey("Unidade"), Column("EstUnidadeId")]
        public long? UnidadeId { get; set; }

        public Unidade Unidade { get; set; }

        [ForeignKey("PrescricaoItem"), Column("AssPrescricaoItemId")]
        public long PrescricaoItemId { get; set; }

        public PrescricaoItem PrescricaoItem { get; set; }

        [ForeignKey("UnidadeRequisicao"), Column("EstUnidadeRequisicaoId")]
        public long? UnidadeRequisicaoId { get; set; }

        public Unidade UnidadeRequisicao { get; set; }

        public int Quantidade { get; set; }
    }
}
