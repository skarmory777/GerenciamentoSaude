using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposRespostas
{
    [Table("AssTipoResposta")]
    public class TipoResposta : CamposPadraoCRUD, IDescricao
    {
        public ICollection<TipoRespostaTipoRespostaConfiguracao> TipoRespostaConfiguracoes { get; set; }
    }
}
