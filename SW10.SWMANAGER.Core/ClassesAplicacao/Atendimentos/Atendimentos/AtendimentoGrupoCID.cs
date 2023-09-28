using SW10.SWMANAGER.ClassesAplicacao.Cadastros.GruposCID;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.Atendimentos
{
    [Table("AteAtendimentoGrupoCID")]
    public class AtendimentoGrupoCID : CamposPadraoCRUD
    {
        public long AtendimentoId { get; set; }

        [ForeignKey("AtendimentoId")]
        public Atendimento Atendimento { get; set; }

        public long GrupoCIDId { get; set; }

        [ForeignKey("GrupoCIDId")]
        public GrupoCID GrupoCID { get; set; }
    }
}
