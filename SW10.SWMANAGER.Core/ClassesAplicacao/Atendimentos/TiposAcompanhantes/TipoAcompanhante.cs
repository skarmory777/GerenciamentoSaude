using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.TiposAcompanhantes
{
    [Table("AteTipoAcompanhante")]
    public class TipoAcompanhante : CamposPadraoCRUD
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override long Id { get; set; }

        public bool IsInternacao { get; set; }
    }
}
