using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabSetor")]
    public class Setor : CamposPadraoCRUD
    {
        public int OrdemSetor { get; set; }
    }
}