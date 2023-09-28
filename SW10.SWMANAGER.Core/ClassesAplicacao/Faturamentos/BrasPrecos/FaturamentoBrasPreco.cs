using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasApresentacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasLaboratorios;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasPrecos
{
    [Table("FatBrasPreco")]
    public class FaturamentoBrasPreco : CamposPadraoCRUD
    {
        [ForeignKey("BrasItemId")]
        public FaturamentoBrasItem BrasItem { get; set; }
        public long BrasItemId { get; set; }

        [ForeignKey("BrasApresentacaoId")]
        public FaturamentoBrasApresentacao BrasApresentacao { get; set; }
        public long? BrasApresentacaoId { get; set; }

        [ForeignKey("BrasLaboratorioId")]
        public FaturamentoBrasLaboratorio BrasLaboratorio { get; set; }
        public long? BrasLaboratorioId { get; set; }

        public double Preco { get; set; }

        public string Tipo { get; set; }

        public string CodigoBrasTiss { get; set; }

        public string CodigoBrasTuss { get; set; }
    }

}


