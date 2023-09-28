using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros
{
    [Table("TipoFrete")]
    public class TipoFrete : CamposPadraoCRUD, IDescricao
    {
    }
}
