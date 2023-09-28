using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [Table("AssTipoRespostaConfig")]
    public class TipoRespostaConfiguracao : CamposPadraoCRUD, IDescricao
    {
        public ICollection<TipoRespostaConfiguracaoElementoHtml> ElementosHtml { get; set; }
    }
}
