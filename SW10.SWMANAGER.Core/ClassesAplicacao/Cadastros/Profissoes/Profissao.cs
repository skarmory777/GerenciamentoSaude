using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Cbos;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Profissoes
{
    [Table("SisProfissao")]
    public class Profissao : CamposPadraoCRUD, IDescricao
    {
        [ForeignKey("SisCbo"), Column("SisCboId")]
        public long? CboId { get; set; }
        public Cbo SisCbo { get; set; }

    }
}
