using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposEntrada
{
    [Table("TipoEntrada")]
    public class TipoEntrada : CamposPadraoCRUD
    {
        public bool IsAtivo { get; set; }
    }
}
