using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabEquipamento")]
    public class Equipamento : CamposPadraoCRUD
    {
        public int TipoLayout { get; set; }
        public string DiretorioOrdem { get; set; }
        public string DiretorioResultado { get; set; }
        //public Informacao Informacao { get; set; }
    }
}