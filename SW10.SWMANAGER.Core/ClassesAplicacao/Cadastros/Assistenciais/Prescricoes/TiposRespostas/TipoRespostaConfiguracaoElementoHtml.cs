using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ElementosHtml;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [Table("AssTipoRespostaConfigElementoHtml")]
    public class TipoRespostaConfiguracaoElementoHtml : CamposPadraoCRUD
    {
        [ForeignKey("ElementoHtml"), Column("SisElementoHtmlId")]
        public long ElementoHtmlId { get; set; }

        public ElementoHtml ElementoHtml { get; set; }

        public string Rotulo { get; set; }

        public string RotuloPosElemento { get; set; }

        [ForeignKey("TipoRespostaConfiguracao"), Column("AssTipoRespostaConfiguracaoId")]
        public long TipoRespostaConfiguracaoId { get; set; }

        public TipoRespostaConfiguracao TipoRespostaConfiguracao { get; set; }

    }
}
