using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class QuitacaoIndex
    {
        public long Id { get; set; }
        public long LancamentoId { get; set; }
        public long IdGrid { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string Documento { get; set; }
        public string Fornecedor { get; set; }
        public decimal? ValorLancamento { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? Acrescimo { get; set; }
        public decimal? MoraMulta { get; set; }
        public decimal? Juros { get; set; }
        public decimal? ValorQuitacao { get; set; }
        public decimal? ValorEfetivo { get; set; }
        public decimal? ValorRestante { get; set; }
        public decimal? ValorRestanteEfetivado { get; set; }
        public DateTime? DataMovimento { get; set; }
        public string MeioPagamento { get; set; }
        public string ContaCorrente { get; set; }
        public string Numero { get; set; }
        public int Parcela { get; set; }
        public bool PossuiEntregaContaRecebida { get; set; }
    }
}
