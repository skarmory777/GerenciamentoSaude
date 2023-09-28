using Abp.Application.Services.Dto;
using SW10.SWMANAGER.ClassesAplicacao.ViewModels;
using System;
using System.Collections.Generic;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.ViewModels
{
    public class VWRptContaPagarDetalhadoDto : EntityDto<long>
    {
        public long? PessoaId { get; set; }
        public string Pessoa { get; set; }
        public long? TipoDocumentoId { get; set; }
        public string TipoDocumento { get; set; }
        public string Numero { get; set; }
        public DateTime? Emissao { get; set; }
        public DateTime? Vencimento { get; set; }
        public long? TipoCobrancaId { get; set; }
        public string TipoCobranca { get; set; }
        public long? SituacaoLancamentoId { get; set; }
        public string SituacaoLancamento { get; set; }
        public long? EmpresaId { get; set; }
        public string Empresa { get; set; }
        public decimal ValorLancamento { get; set; }
        public decimal Juros { get; set; }
        public decimal Multa { get; set; }
        public decimal Correcao { get; set; }
        public decimal Descontos { get; set; }
        public decimal ValorLiquido { get; set; }
        public decimal ValorBrutoPago { get; set; }
        public decimal JurosQuitacao { get; set; }
        public decimal MultaQuitacao { get; set; }
        public decimal AcrescimoQuitacao { get; set; }
        public decimal DescontoQuitacao { get; set; }
        public decimal ValorPagoLiquido { get; set; }
        public decimal ValorPagar { get; set; }
        public bool IsCredito { get; set; }

        #region Mapeamento
        public static VWRptContaPagarDetalhadoDto Mapear(VWRptContaPagarDetalhado item)
        {
            var result = new VWRptContaPagarDetalhadoDto
            {
                AcrescimoQuitacao = item.AcrescimoQuitacao,
                Correcao = item.Correcao,
                DescontoQuitacao = item.DescontoQuitacao,
                Descontos = item.Descontos,
                Emissao = item.Emissao,
                Id = item.Id,
                Juros = item.Juros,
                JurosQuitacao = item.JurosQuitacao,
                Multa = item.Multa,
                MultaQuitacao = item.MultaQuitacao,
                Numero = item.Numero,
                Pessoa = item.Pessoa,
                TipoCobranca = item.TipoCobranca,
                TipoDocumento = item.TipoDocumento,
                ValorBrutoPago = item.ValorBrutoPago,
                ValorLancamento = item.ValorLancamento,
                ValorLiquido = item.ValorLiquido,
                ValorPagar = item.ValorPagar,
                ValorPagoLiquido = item.ValorPagoLiquido,
                Vencimento = item.Vencimento,
                Empresa = item.Empresa,
                EmpresaId = item.EmpresaId,
                PessoaId = item.PessoaId,
                SituacaoLancamento = item.SituacaoLancamento,
                SituacaoLancamentoId = item.SituacaoLancamentoId,
                TipoCobrancaId = item.TipoCobrancaId,
                TipoDocumentoId = item.TipoDocumentoId,
                IsCredito = item.IsCredito
            };
            return result;
        }

        public static VWRptContaPagarDetalhado Mapear(VWRptContaPagarDetalhadoDto item)
        {
            var result = new VWRptContaPagarDetalhado
            {
                AcrescimoQuitacao = item.AcrescimoQuitacao,
                Correcao = item.Correcao,
                DescontoQuitacao = item.DescontoQuitacao,
                Descontos = item.Descontos,
                Emissao = item.Emissao,
                Id = item.Id,
                Juros = item.Juros,
                JurosQuitacao = item.JurosQuitacao,
                Multa = item.Multa,
                MultaQuitacao = item.MultaQuitacao,
                Numero = item.Numero,
                Pessoa = item.Pessoa,
                TipoCobranca = item.TipoCobranca,
                TipoDocumento = item.TipoDocumento,
                ValorBrutoPago = item.ValorBrutoPago,
                ValorLancamento = item.ValorLancamento,
                ValorLiquido = item.ValorLiquido,
                ValorPagar = item.ValorPagar,
                ValorPagoLiquido = item.ValorPagoLiquido,
                Vencimento = item.Vencimento,
                Empresa = item.Empresa,
                EmpresaId = item.EmpresaId,
                PessoaId = item.PessoaId,
                SituacaoLancamento = item.SituacaoLancamento,
                SituacaoLancamentoId = item.SituacaoLancamentoId,
                TipoCobrancaId = item.TipoCobrancaId,
                TipoDocumentoId = item.TipoDocumentoId,
                IsCredito = item.IsCredito
            };
            return result;
        }

        public static IEnumerable<VWRptContaPagarDetalhadoDto> Mapear(List<VWRptContaPagarDetalhado> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }

        public static IEnumerable<VWRptContaPagarDetalhado> Mapear(List<VWRptContaPagarDetalhadoDto> list)
        {
            foreach (var item in list)
            {
                yield return Mapear(item);
            }
        }
        #endregion
    }
}
