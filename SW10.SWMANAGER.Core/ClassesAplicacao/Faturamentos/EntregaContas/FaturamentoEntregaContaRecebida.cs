using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Financeiros;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.EntregaContas
{
    [Table("FatEntregaContaRecebida")]
    public class FaturamentoEntregaContaRecebida : CamposPadraoCRUD
    {
        [ForeignKey("Quitacao"), Column("FinQuitacaoId")]
        public long? QuitacaoId { get; set; }
        public Quitacao Quitacao { get; set; }


        [ForeignKey("FaturamentoEntregaConta"), Column("FatEntregaContaId")]
        public long? FaturamentoEntregaContaId { get; set; }
        public FaturamentoEntregaConta FaturamentoEntregaConta { get; set; }

        public float ValorRecebido { get; set; }

        public float? ValorGlosaRecuperavel { get; set; }

        public float? ValorGlosaIrrecuperavel { get; set; }
    }
}
