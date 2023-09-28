using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    [Table("SisUltimoId")]
    public class UltimoId : CamposPadraoCRUD
    {
        public string NomeTabela { get; set; }
        public int? TamanhoCampo { get; set; }

        [MaxLength(1)]
        public string complementoEsquerda { get; set; }

    }
}
