using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos
{
    [Table("SisMovAutomaticoTipoGuia")]
    public class MovimentoAutomaticoTipoGuia : CamposPadraoCRUD
    {
        public long MovimentoAutomaticoId { get; set; }
        public long FaturamentoGuiaId { get; set; }

        [ForeignKey("MovimentoAutomaticoId")]
        public MovimentoAutomatico MovimentoAutomatico { get; set; }

        [ForeignKey("FaturamentoGuiaId")]
        public FaturamentoGuia FaturamentoGuia { get; set; }

    }
}
