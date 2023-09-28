using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Fornecedores.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    public class EstoqueImportacaoProdutoDto //: CamposPadraoCRUDDto
    {
        public long? FornecedorId { get; set; }
        public long? ProdutoId { get; set; }

        public string CodigoProdutoNota { get; set; }
        public string DescricaoProdutoNota { get; set; }
        public string InformacaoAdicionalNota { get; set; }
        public string DescricaoProduto { get; set; }
        public string CNPJNota { get; set; }

        public FornecedorDto Fornecedor { get; set; }
        public ProdutoDto Produto { get; set; }

        public int Index { get; set; }
        public string UnidadeNota { get; set; }
        public long UnidadeId { get; set; }
        public decimal? Fator { get; set; }

        public string Lote { get; set; }
        public DateTime Validade { get; set; }
        public string Serie { get; set; }
        public decimal Quantidade { get; set; }
        public decimal CustoUnitario { get; set; }

        public decimal PercentualIPI { get; set; }
        public decimal ValorIPI { get; set; }

        public decimal PercentualICMS { get; set; }
        public decimal ValorICMS { get; set; }

        public long? TransportadoraId { get; set; }
        public FornecedorDto Transportadora { get; set; }

        public List<RastroDto> Rastros { get; set; }
    }


    public class EstoqueImportacaoProdutoListarDto :CamposPadraoCRUDDto
    {
        public long ProdutoId { get; set; }
        public string ProdutoDescricao { get; set; }
        public string CNPJNota { get; set; }

        public long? UnidadeId { get; set; }

        public string UnidadeDescricao { get; set; }

        public decimal? Fator { get; set; }

        public long? FornecedorId { get; set; }

        public string ForncecedorNomeFantasia { get; set; }

    }
}
