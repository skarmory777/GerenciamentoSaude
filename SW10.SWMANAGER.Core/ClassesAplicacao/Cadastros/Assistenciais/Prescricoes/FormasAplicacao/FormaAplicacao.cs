using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao
{
    [Table("AssFormaAplicacao")]
    public class FormaAplicacao : CamposPadraoCRUD, IDescricao
    {
    }
}
