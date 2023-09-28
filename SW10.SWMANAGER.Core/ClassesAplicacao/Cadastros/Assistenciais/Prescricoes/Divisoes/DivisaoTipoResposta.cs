using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes
{
    [Table("AssDivisaoTipoResposta")]
    public class DivisaoTipoResposta : CamposPadraoCRUD
    {
        [ForeignKey("Divisao"), Column("AssDivisaoId")]
        public long DivisaoId { get; set; }

        [ForeignKey("TipoResposta"), Column("AssTipoRespostaId")]
        public long TipoRespostaId { get; set; }

        public Divisao Divisao { get; set; }

        public TipoResposta TipoResposta { get; set; }
    }
}
