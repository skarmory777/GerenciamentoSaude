using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    public class LancamentoIndex
    {
        public long? Id { get; set; }
        public Guid? AnexoListaId { get; set; }
        public string SituacaoDescricao { get; set; }
        public string CorLancamentoFundo { get; set; }
        public string CorLancamentoLetra { get; set; }
        public DateTimeOffset? DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public string Competencia { get; set; }
        public string Documento { get; set; }
        public string Fornecedor { get; set; }
        public decimal? TotalLancamento { get; set; }
        public string CodigoBarras { get; set; }
        public string TipoDocumento { get; set; }
        public string ContaAdministrativa { get; set; }
        public bool IsCredito { get; set; }
        public long SituacaoLancamentoId { get; set; }
        public decimal? ValorLancamento { get; set; }
        public int? AnoCompetencia { get; set; }
        public DateTimeOffset? DataLancamento { get; set; }
        public decimal? Juros { get; set; }
        public int? MesCompetencia { get; set; }
        public decimal? Multa { get; set; }
        public int Parcela { get; set; }
        public decimal? ValorAcrescimoDecrescimo { get; set; }
        public string NossoNumero { get; set; }
        public string LinhaDigitavel { get; set; }
        public bool IsSelecionado { get; set; }
        public long IdGrid { get; set; }
        public decimal? TotalQuitacao { get; set; }
        public string EmpresaNome { get; set; }
        public long? FatEntregaLoteId { get; set; }
    }

}
