using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Fornecedores;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Unidades.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.ProdutosLaboratorio.Dto;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Compras.Dto
{
    public class CompraCotacaoFornecedorDto
    {
        public long RequisicaoId { get; set; }
        public long RequisicaoItemId { get; set; }
        public long ProdutoId { get; set; }
        public ProdutoDto Produto { get; set; }
        public decimal Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public long FornecedorId { get; set; }
        public long? FormaPagamentoId { get; set; }
        public FormaPagamentoDto FormaPagamento { get; set; }
        public int? PrazoEntregaFornecedorEmDias { get; set; }
        public int? PrazoEntregaEmDias { get; set; }
        public SisFornecedorDto Fornecedor { get; set; }
        public long? UnidadeId { get; set; }
        public UnidadeDto Unidade { get; set; }
        public long? LaboratorioId { get; set; }
        public ProdutoLaboratorioDto Laboratorio { get; set; }
        public bool? OpcaoComprador { get; set; }
        public long? IdGrid { get; set; }
    }
}
