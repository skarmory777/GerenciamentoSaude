using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Assistenciais.Prescricoes.PrescricoesItens.Dto
{
    [AutoMap(typeof(FormulaEstoque))]
    public class FormulaEstoqueDto : CamposPadraoCRUDDto, IDescricao
    {
        public long? EstoqueId { get; set; }

        public EstoqueDto EstoqueOrigem { get; set; }

        public long? ProdutoId { get; set; }

        public ProdutoDto Produto { get; set; }

        public long? UnidadeId { get; set; }

        public UnidadeDto Unidade { get; set; }

        public long? UnidadeRequisicaoId { get; set; }

        public UnidadeDto UnidadeRequisicao { get; set; }

        public long PrescricaoItemId { get; set; }

        public PrescricaoItemDto PrescricaoItem { get; set; }

        public bool IsPrincipal { get; set; }

        public long? IdGridFormulasEstoque { get; set; }

        public int Quantidade { get; set; }

        public string ProdutoDescricao { get; set; }
        public string UnidadeDescricao { get; set; }


        //public ICollection<FormulaEstoqueItemDto> FormulaEstoqueItens { get; set; }

    }
}
