using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class InformacaoLoteValidadeTodosDto
    {
        public string DescricaoProdutoNota { get; set; }
        public long? LoteValidadeId { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }
        public long? LaboratorioId { get; set; }
        public ProdutoLaboratorioDto Laboratorio { get; set; }
        public long ProdutoId { get; set; }
        public string CodigoProdutoNota { get; set; }
        public long? EstoquePreMovimentoLoteValidadeId { get; set; }
        public string DescricaoProduto { get; set; }
        public long PreMovimentoItemId { get; set; }
        public decimal Quantidade { get; set; }

        public int Index { get; set; }
    }
}
