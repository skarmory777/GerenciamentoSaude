using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Services.Cadastros.Produtos.Dto;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques.Movimentos;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Suprimentos.Estoques.Movimentos.Dto
{
    [AutoMap(typeof(EstoqueMovimentoItem))]
    public class MovimentoItemDto
    {
        public long Id { get; set; }
        public string Produto { get; set; }
        public decimal Quantidade { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }
        public string Laboratorio { get; set; }
        public bool IsValidade { get; set; }
        public long ProdutoId { get; set; }
        public string Unidade { get; set; }
        public decimal CustoUnitario { get; set; }
        public decimal CustoTotal { get; set; }
        public long TransferenciaItemId { get; set; }
        public bool IsLote { get; set; }
        public string NumeroSerie { get; set; }
        public string Fornecedor { get; set; }
        public decimal PerIPI { get; set; }
        public decimal ValorIPI { get; set; }

        public decimal PerICMS { get; set; }
        public decimal ValorICMS { get; set; }
        public ProdutoDto ProdutoDto { get; set; }
        public long? ProdutoUnidadeId { get; set; }
        public long MovimentoId { get; set; }
        public decimal? QuantidadeBaixa { get; set; }
        public long? BaixaItemId { get; set; }
        public long? EstoqueKitItemId { get; set; }
        public List<LoteValidadesGrid> LoteValidades { get; set; }
        
        // public long? IdGrid { get; set; }
    }

    public class LoteValidadesGrid
    {
        public long Id { get; set; }

        public long LoteValidadeId { get; set; }

        public string Lote { get; set; }

        public DateTime? Validade { get; set; }

        public decimal Quantidade { get; set; }
    }
}
