using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinMeioPagamento")]
    public class MeioPagamento : CamposPadraoCRUD
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

        [ForeignKey("TipoMeioPagamentoId")]
        public TipoMeioPagamento TipoMeioPagamento { get; set; }
    }
}
