using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabTecnico")]
    public class Tecnico : CamposPadraoCRUD
    {
        public string RegConselho { get; set; }
    }
}