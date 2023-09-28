using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class VWRptEstoqueMovimentoDetalhadoDto : EntityDto<long>
    {
        public long? EstoqueId { get; set; }
        public string Estoque { get; set; }
        public long? GrupoId { get; set; }
        public string Grupo { get; set; }
        public long? ProdutoId { get; set; }
        public string CodProduto { get; set; }
        public string Produto { get; set; }
        public string Unidade { get; set; }
        public string Documento { get; set; }
        public DateTime Data { get; set; }
        public decimal QuantidadeEntrada { get; set; }
        public decimal QuantidadeSaida { get; set; }
        public decimal CustoUnitario { get; set; }
        public string NumeroSerie { get; set; }
        public string TipoMovimento { get; set; }
        public bool IsEntrada { get; set; }
        public long? EstTipoMovimentoId { get; set; }
        public string TipoOperacao { get; set; }
        public string CentroCusto { get; set; }
        public string Lote { get; set; }
        public DateTime? Validade { get; set; }
        public string Pessoa { get; set; }
        public decimal SaldoInicial { get; set; }
        public decimal SaldoFinal { get; set; }

        #region Mapeamento
        public static VWRptEstoqueMovimentoDetalhadoDto Mapear(VWRptEstoqueMovimentoDetalhado input)
        {
            var result = new VWRptEstoqueMovimentoDetalhadoDto();
            result.CodProduto = input.CodProduto;
            result.CustoUnitario = input.CustoUnitario;
            result.Data = input.Data;
            result.Estoque = input.Estoque;
            result.EstoqueId = input.EstoqueId;
            result.Grupo = input.Grupo;
            result.GrupoId = input.GrupoId;
            result.NumeroSerie = input.NumeroSerie;
            result.Produto = input.Produto;
            result.ProdutoId = input.ProdutoId;
            result.CentroCusto = input.CentroCusto;
            result.Documento = input.Documento;
            result.EstTipoMovimentoId = input.EstTipoMovimentoId;
            result.IsEntrada = input.IsEntrada;
            result.Lote = input.Lote;
            result.Pessoa = input.Pessoa;
            result.Unidade = input.Unidade;
            result.Validade = input.Validade;
            result.QuantidadeEntrada = input.QuantidadeEntrada;
            result.QuantidadeSaida = input.QuantidadeSaida;
            result.TipoOperacao = input.TipoOperacao;

            return result;
        }
        public static VWRptEstoqueMovimentoDetalhado Mapear(VWRptEstoqueMovimentoDetalhadoDto input)
        {
            var result = new VWRptEstoqueMovimentoDetalhado();
            result.CodProduto = input.CodProduto;
            result.CustoUnitario = input.CustoUnitario;
            result.Data = input.Data;
            result.Estoque = input.Estoque;
            result.EstoqueId = input.EstoqueId;
            result.Grupo = input.Grupo;
            result.GrupoId = input.GrupoId;
            result.NumeroSerie = input.NumeroSerie;
            result.Produto = input.Produto;
            result.ProdutoId = input.ProdutoId;
            result.CentroCusto = input.CentroCusto;
            result.Documento = input.Documento;
            result.EstTipoMovimentoId = input.EstTipoMovimentoId;
            result.IsEntrada = input.IsEntrada;
            result.Lote = input.Lote;
            result.Pessoa = input.Pessoa;
            result.Unidade = input.Unidade;
            result.Validade = input.Validade;
            result.QuantidadeEntrada = input.QuantidadeEntrada;
            result.QuantidadeSaida = input.QuantidadeSaida;
            result.TipoOperacao = input.TipoOperacao;

            return result;
        }
        public static IEnumerable<VWRptEstoqueMovimentoDetalhadoDto> Mapear(List<VWRptEstoqueMovimentoDetalhado> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        public static IEnumerable<VWRptEstoqueMovimentoDetalhado> Mapear(List<VWRptEstoqueMovimentoDetalhadoDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion

    }
}
