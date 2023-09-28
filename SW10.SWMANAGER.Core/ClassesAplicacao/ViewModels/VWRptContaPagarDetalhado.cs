using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.ViewModels
{
    [Table("vwRptContaPagarDetalhado")]
    public class VWRptContaPagarDetalhado : Entity<long>
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
    }
}
