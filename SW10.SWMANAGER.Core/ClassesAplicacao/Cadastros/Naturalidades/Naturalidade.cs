using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Naturalidades
{
    [Table("SisNaturalidade")]
    public class Naturalidade : CamposPadraoCRUD, IDescricao
    {
    }
}
