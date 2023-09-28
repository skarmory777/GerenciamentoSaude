using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinLancamentoQuitacao")]
    public class LancamentoQuitacao : CamposPadraoCRUD
    {
        public long LancamentoId { get; set; }

        [ForeignKey(("LancamentoId"))]
        public Lancamento Lancamento { get; set; }

        public long QuitacaoId { get; set; }

        [ForeignKey("QuitacaoId")]
        public Quitacao Quitacao { get; set; }

        public decimal? Desconto { get; set; }
        public decimal? Acrescimo { get; set; }
        public decimal? MoraMulta { get; set; }
        public decimal ValorQuitacao { get; set; }
        public decimal ValorEfetivo { get; set; }
        public decimal? Juros { get; set; }

    }
}
