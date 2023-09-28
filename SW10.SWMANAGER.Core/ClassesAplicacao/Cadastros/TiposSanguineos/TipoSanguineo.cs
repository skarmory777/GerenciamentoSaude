using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposSanguineos
{
    [Table("SisTipoSanguineo")]
    public class TipoSanguineo : CamposPadraoCRUD, IDescricao
    {
    }
}
