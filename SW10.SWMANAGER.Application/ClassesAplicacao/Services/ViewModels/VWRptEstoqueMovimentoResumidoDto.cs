using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class VWRptEstoqueMovimentoResumidoDto : EntityDto<long>
    {
        public long? EstoqueId { get; set; }
        public string Estoque { get; set; }
        public long? GrupoId { get; set; }
        public string Grupo { get; set; }
        public long? ProdutoId { get; set; }
        public string CodProduto { get; set; }
        public string Produto { get; set; }
        public decimal QtdSaldoInicial { get; set; }
        //public string Unidade { get; set; }
        //public DateTime Data { get; set; }
        public decimal QtdEntrada { get; set; }
        public decimal QtdSaida { get; set; }
        public decimal QtdFinal { get; set; }
        public decimal QtdEntradaApos { get; set; }
        public decimal QtdSaidaApos { get; set; }
        //public decimal CustoUnitario { get; set; }
        //public string NumeroSerie { get; set; }
        public decimal QtdSaldoAtual { get; set; }

        #region Mapeamento
        public static VWRptEstoqueMovimentoResumidoDto Mapear(VWRptEstoqueMovimentoResumido input)
        {
            var result = new VWRptEstoqueMovimentoResumidoDto();
            result.CodProduto = input.CodProduto;
            //result.CustoUnitario = input.CustoUnitario;
            //result.Data = input.Data;
            result.EstoqueId = input.EstoqueId;
            result.Estoque = input.Estoque;
            result.GrupoId = input.GrupoId;
            result.Grupo = input.Grupo;
            //result.NumeroSerie = input.NumeroSerie;
            //result.Unidade = input.Unidade;
            result.ProdutoId = input.ProdutoId;
            result.Produto = input.Produto;
            result.QtdSaldoInicial = input.QtdSaldoInicial;
            result.QtdEntrada = input.QtdEntrada;
            result.QtdSaida = input.QtdSaida;
            result.QtdFinal = input.QtdFinal;
            result.QtdEntradaApos = input.QtdEntradaApos;
            result.QtdSaidaApos = input.QtdSaidaApos;
            result.QtdSaldoAtual = input.QtdSaldoAtual;

            return result;
        }
        public static VWRptEstoqueMovimentoResumido Mapear(VWRptEstoqueMovimentoResumidoDto input)
        {
            var result = new VWRptEstoqueMovimentoResumido();
            result.CodProduto = input.CodProduto;
            //result.CustoUnitario = input.CustoUnitario;
            //result.Data = input.Data;
            result.EstoqueId = input.EstoqueId;
            result.Estoque = input.Estoque;
            result.GrupoId = input.GrupoId;
            result.Grupo = input.Grupo;
            //result.NumeroSerie = input.NumeroSerie;
            //result.Unidade = input.Unidade;
            result.ProdutoId = input.ProdutoId;
            result.Produto = input.Produto;
            result.QtdSaldoInicial = input.QtdSaldoInicial;
            result.QtdEntrada = input.QtdEntrada;
            result.QtdSaida = input.QtdSaida;
            result.QtdFinal = input.QtdFinal;
            result.QtdEntradaApos = input.QtdEntradaApos;
            result.QtdSaidaApos = input.QtdSaidaApos;
            result.QtdSaldoAtual = input.QtdSaldoAtual;

            return result;
        }
        public static IEnumerable<VWRptEstoqueMovimentoResumidoDto> Mapear(List<VWRptEstoqueMovimentoResumido> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        public static IEnumerable<VWRptEstoqueMovimentoResumido> Mapear(List<VWRptEstoqueMovimentoResumidoDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion
    }
}
