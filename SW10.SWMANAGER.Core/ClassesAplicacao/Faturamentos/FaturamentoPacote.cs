using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos
{
    [Table("FatPacote")]
    public class FaturamentoPacote : CamposPadraoCRUD
    {
        [Index("Fat_Idx_Inicio")]
        public DateTime Inicio { get; set; }
        [Index("Fat_Idx_Final")]
        public DateTime Final { get; set; }

        public long? FaturamentoItemId { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }

        public long? FaturamentoContaId { get; set; }

        [ForeignKey("FaturamentoContaId")]
        public FaturamentoConta FaturamentoConta { get; set; }

        public float? Qtde { get; set; }
    }
}
