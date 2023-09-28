using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(Lancamento))]
    public class LancamentoDto : CamposPadraoCRUDDto
    {
        public Guid? AnexoListaId { get; set; }
        public long DocumentoId { get; set; }
        public DocumentoDto Documento { get; set; }
        public DateTime? DataVencimento { get; set; }
        public decimal? ValorLancamento { get; set; }
        public decimal? ValorAcrescimoDecrescimo { get; set; }
        public decimal? Juros { get; set; }
        public decimal? Multa { get; set; }
        public long SituacaoLancamentoId { get; set; }
        public SituacaoLancamentoDto SituacaoLancamento { get; set; }
        public long? DocumentoRelacionadoId { get; set; }
        public DocumentoDto DocumentoRelacionado { get; set; }
        public int? MesCompetencia { get; set; }
        public int? AnoCompetencia { get; set; }
        public bool IsCredito { get; set; }
        public DateTimeOffset? DataLancamento { get; set; }
        public int Parcela { get; set; }
        public string SituacaoDescricao { get; set; }
        public decimal Total { get; set; }
        public long IdGrid { get; set; }
        public string CorLancamentoFundo { get; set; }
        public string CorLancamentoLetra { get; set; }
        public bool IsSelecionado { get; set; }
        public string NossoNumero { get; set; }
        public string CodigoBarras { get; set; }
        public string LinhaDigitavel { get; set; }
        public decimal? ValorRestante { get; set; }
    }
}
