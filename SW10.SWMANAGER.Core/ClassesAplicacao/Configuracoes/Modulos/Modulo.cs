using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Modulos
{
    [Table("SisModulo")]
    public class Modulo : CamposPadraoCRUD, IDescricao
    {
    }
}
