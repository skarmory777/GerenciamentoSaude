using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [Table("AssTipoRespostaAssTipoRespostaConfig")]
    public class TipoRespostaTipoRespostaConfiguracao : CamposPadraoCRUD
    {
        [ForeignKey("TipoResposta"), Column("AssTipoRespostaId")]
        public long TipoRespostaId { get; set; }

        public TipoResposta TipoResposta { get; set; }


        [ForeignKey("TipoRespostaConfiguracao"), Column("AssTipoRespostaConfigId")]
        public long TipoRespostaConfiguracaoId { get; set; }

        public TipoRespostaConfiguracao TipoRespostaConfiguracao { get; set; }
    }
}
