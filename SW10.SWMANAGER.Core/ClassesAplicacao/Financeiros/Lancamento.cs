using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinLancamento")]
    public class Lancamento : CamposPadraoCRUD
    {
        public long? DocumentoId { get; set; }

        [ForeignKey("DocumentoId")]
        public Documento Documento { get; set; }
        [Index("Fin_Idx_DataVencimento")]
        public DateTime? DataVencimento { get; set; }
        public decimal? ValorLancamento { get; set; }
        public decimal? ValorAcrescimoDecrescimo { get; set; }
        public decimal? Juros { get; set; }
        public decimal? Multa { get; set; }

        public long SituacaoLancamentoId { get; set; }

        [ForeignKey("SituacaoLancamentoId")]
        public SituacaoLancamento SituacaoLancamento { get; set; }

        public int? MesCompetencia { get; set; }
        public int? AnoCompetencia { get; set; }
        [Index("Fin_Idx_IsCredito")]
        public bool IsCredito { get; set; }
        [Index("Fin_Idx_DataLancamento")]
        public DateTimeOffset? DataLancamento { get; set; }
        public int Parcela { get; set; }

        public string NossoNumero { get; set; }
        public string CodigoBarras { get; set; }
        public string LinhaDigitavel { get; set; }

        public List<LancamentoQuitacao> LancamentosQuitacoes { get; set; }

        public Guid? AnexoListaId { get; set; }
    }
}
