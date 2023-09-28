using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos
{
    [Table("SisMovFaturamentoItem")]
    public class MovimentoAutomaticoFaturamentoItem : CamposPadraoCRUD
    {
        public long MovimentoAutomaticoId { get; set; }
        public long FaturamentoItemId { get; set; }

        [ForeignKey("MovimentoAutomaticoId")]
        public MovimentoAutomatico MovimentoAutomatico { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }
    }
}
