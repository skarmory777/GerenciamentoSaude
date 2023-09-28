using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabMaterial")]
    public class Material : CamposPadraoCRUD
    {
        public int Ordem { get; set; }
        
        public bool IsDescriminaLocal { get; set; }
    }
}
