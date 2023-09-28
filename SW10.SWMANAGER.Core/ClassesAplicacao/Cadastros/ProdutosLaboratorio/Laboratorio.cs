using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProdutosLaboratorio
{
    [Table("EstLaboratorio")]
    public class EstoqueLaboratorio : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoBrasLaboratorio"), Column("FatBrasLaboratorioId")]
        public long? BrasLaboratorioId { get; set; }

        public FaturamentoBrasLaboratorio FaturamentoBrasLaboratorio { get; set; }

    }
}
