using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.MotivosAlta
{
    [Table("AssMotivoAlta")]
    public class MotivoAlta : CamposPadraoCRUD
    {
        [ForeignKey("MotivoAltaTipoAlta"), Column("AssMotivoAltaTipoAltaId")]
        public long MotivoAltaTipoAltaId { get; set; }

        public MotivoAltaTipoAlta MotivoAltaTipoAlta { get; set; }
    }
}
