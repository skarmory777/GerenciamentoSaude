using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Planos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos
{
    [Table("SisMovAutomaticoConvenioPlano")]
    public class MovimentoAutomaticoConvenioPlano : CamposPadraoCRUD
    {
        public long MovimentoAutomaticoId { get; set; }
        public long? ConvenioId { get; set; }
        public long? PlanoId { get; set; }

        [ForeignKey("MovimentoAutomaticoId")]
        public MovimentoAutomatico MovimentoAutomatico { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        [ForeignKey("PlanoId")]
        public Plano Plano { get; set; }
    }
}
