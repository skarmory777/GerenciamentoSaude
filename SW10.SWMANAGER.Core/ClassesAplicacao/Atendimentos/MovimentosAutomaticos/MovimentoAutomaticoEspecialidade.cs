using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Especialidades;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.MovimentosAutomaticos
{
    [Table("SisMovAutomaticoEspecialidade")]
    public class MovimentoAutomaticoEspecialidade : CamposPadraoCRUD
    {
        public long MovimentoAutomaticoId { get; set; }
        public long EspecialidadeId { get; set; }

        [ForeignKey("MovimentoAutomaticoId")]
        public MovimentoAutomatico MovimentoAutomatico { get; set; }

        [ForeignKey("EspecialidadeId")]
        public Especialidade Especialidade { get; set; }
    }
}
