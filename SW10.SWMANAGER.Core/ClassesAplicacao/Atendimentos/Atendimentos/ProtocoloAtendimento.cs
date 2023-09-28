using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    [Table("AteProtocoloAtendimento")]
    public class ProtocoloAtendimento : CamposPadraoCRUD
    {
        public long? ClassificacaoAtendimentoId { get; set; }

        [ForeignKey("ClassificacaoAtendimentoId")]
        public ClassificacaoAtendimento ClassificacaoAtendimento { get; set; }
    }
}
