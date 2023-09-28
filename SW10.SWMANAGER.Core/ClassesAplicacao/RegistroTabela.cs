using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao
{
    [Table("SisRegistroTabela")]
    public class RegistroTabela : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        [MaxLength(500)]
        [Index("Sis_Idx_TabelaPrincipal")]
        public string TabelaPrincipal { get; set; }
    }
}
