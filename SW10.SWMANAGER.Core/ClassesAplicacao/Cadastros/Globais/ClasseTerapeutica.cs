using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Globais
{
    [Table("Gbl_ClasseTerapeutica")]
    public class ClasseTerapeutica : CamposPadraoCRUD
    {
        public string CAS { get; set; }
    }
}
