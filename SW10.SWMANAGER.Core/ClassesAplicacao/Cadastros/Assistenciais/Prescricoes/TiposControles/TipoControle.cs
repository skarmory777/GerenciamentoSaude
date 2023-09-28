using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.TiposControles
{
    [Table("AssTipoControle")]
    public class TipoControle : CamposPadraoCRUD, IDescricao
    {
    }
}
