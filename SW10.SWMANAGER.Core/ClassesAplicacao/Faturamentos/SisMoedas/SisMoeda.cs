using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.SisMoedas
{
    [Table("SisMoeda")]
    public class SisMoeda : CamposPadraoCRUD
    {
        // 1 - fixa
        // 2 - variavel por convenio


        public static long Real = 1;

        public int Tipo { get; set; }

        public bool IsCobraCoch { get; set; }
    }
}

