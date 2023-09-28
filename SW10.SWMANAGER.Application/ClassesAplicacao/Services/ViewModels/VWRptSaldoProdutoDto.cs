using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;

using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class VWRptSaldoProdutoDto : EntityDto<long>
    {
        public long? EstoqueId { get; set; }
        public string Estoque { get; set; }
        public long? GrupoId { get; set; }
        public string Grupo { get; set; }
        public long? ProdutoId { get; set; }
        public string Produto { get; set; }
        public decimal QuantidadeAtual { get; set; }
        public decimal QuantidadeEntradaPendente { get; set; }
        public decimal QuantidadeSaidaPendente { get; set; }
        public decimal SaldoFuturo { get; set; }
        public string UnidadeReferencial { get; set; }
        public string UnidadeGerencial { get; set; }

        #region Mapeamento
        public static VWRptSaldoProdutoDto Mapear(VWRptSaldoProduto input)
        {
            var result = new VWRptSaldoProdutoDto();
            result.Grupo = input.Grupo;
            result.GrupoId = input.GrupoId;
            result.Id = input.Id;
            result.Produto = input.Produto;
            result.ProdutoId = input.ProdutoId;
            result.QuantidadeAtual = input.QuantidadeAtual;
            result.QuantidadeEntradaPendente = input.QuantidadeEntradaPendente;
            result.QuantidadeSaidaPendente = input.QuantidadeSaidaPendente;
            result.SaldoFuturo = input.SaldoFuturo;
            result.EstoqueId = input.EstoqueId;
            result.Estoque = input.Estoque;
            result.UnidadeGerencial = input.UnidadeGerencial;
            result.UnidadeReferencial = input.UnidadeReferencial;

            return result;
        }
        public static VWRptSaldoProduto Mapear(VWRptSaldoProdutoDto input)
        {
            var result = new VWRptSaldoProduto();
            result.Grupo = input.Grupo;
            result.GrupoId = input.GrupoId;
            result.Id = input.Id;
            result.Produto = input.Produto;
            result.ProdutoId = input.ProdutoId;
            result.QuantidadeAtual = input.QuantidadeAtual;
            result.QuantidadeEntradaPendente = input.QuantidadeEntradaPendente;
            result.QuantidadeSaidaPendente = input.QuantidadeSaidaPendente;
            result.SaldoFuturo = input.SaldoFuturo;
            result.EstoqueId = input.EstoqueId;
            result.Estoque = input.Estoque;
            result.UnidadeGerencial = input.UnidadeGerencial;
            result.UnidadeReferencial = input.UnidadeReferencial;

            return result;
        }
        public static IEnumerable<VWRptSaldoProdutoDto> Mapear(List<VWRptSaldoProduto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        public static IEnumerable<VWRptSaldoProduto> Mapear(List<VWRptSaldoProdutoDto> list)
        {
            foreach (var input in list)
            {
                yield return Mapear(input);
            }
        }
        #endregion
    }
}
