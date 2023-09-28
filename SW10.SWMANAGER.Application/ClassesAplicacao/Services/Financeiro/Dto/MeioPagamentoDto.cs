using Abp.AutoMapper;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;

namespace SW10.SWMANAGER.ClassesAplicacao.Services.Financeiro.Dto
{
    [AutoMap(typeof(MeioPagamento))]
    public class MeioPagamentoDto : CamposPadraoCRUDDto
    {
        public int? DiasRetencaoDebito { get; set; }
        public int? DiasRetencaoCredito { get; set; }
        public decimal? TaxaAdministracao { get; set; }
        public string MascaraCredito { get; set; }
        public string MascaraDebito { get; set; }
        public string DescricaoMascaraCredito { get; set; }
        public string DescricaoMascaraDebito { get; set; }
        public bool IsNumeroDocumentoObrigatorio { get; set; }
        public bool IsPagamentoEletronico { get; set; }
        public long TipoMeioPagamentoId { get; set; }
        public TipoMeioPagamentoDto TipoMeioPagamento { get; set; }
    }
}
