using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.PaineisSenha
{
    [Table("AteSenhaMovPainel")]
    public class SenhaMovimentacaoPainel : CamposPadraoCRUD
    {
        public long PainelId { get; set; }
        public long SenhaMovimentacaoId { get; set; }

        [ForeignKey("PainelId")]
        public Painel Painel { get; set; }

        [ForeignKey("SenhaMovimentacaoId")]
        public SenhaMovimentacao SenhaMovimentacao { get; set; }

        public bool IsMostra { get; set; }
    }
}
