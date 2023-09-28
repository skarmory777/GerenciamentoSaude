using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabResultadoStatus")]
    public class ResultadoStatus : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        public string CorFonte { get; set; }
        public string CorFundo { get; set; }
        public int Sequencia { get; set; }
        public bool IsAtivo { get; set; }
    }
}
