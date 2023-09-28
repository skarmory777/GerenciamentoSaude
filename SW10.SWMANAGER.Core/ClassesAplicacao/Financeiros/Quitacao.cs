using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Financeiros
{
    [Table("FinQuitacao")]
    public class Quitacao : CamposPadraoCRUD
    {
        public string Numero { get; set; }
        [Index("Fin_Idx_DataMovimento")]
        public DateTime? DataMovimento { get; set; }
        [Index("Fin_Idx_DataCompensado")]
        public DateTime? DataCompensado { get; set; }
        [Index("Fin_Idx_DataConsolidado")]
        public DateTime? DataConsolidado { get; set; }
        public string Observacao { get; set; }
        public Guid? TransferenciaIdentificador { get; set; }
        public decimal? ValorImpostos { get; set; }


        [ForeignKey("ContaCorrenteId")]
        public ContaCorrente ContaCorrente { get; set; }
        public long ContaCorrenteId { get; set; }

     
        [ForeignKey("MeioPagamentoId")]
        public MeioPagamento MeioPagamento { get; set; }
        public long MeioPagamentoId { get; set; }
                

        [ForeignKey("ChequeId")]
        public Cheque Cheque { get; set; }
        public long? ChequeId { get; set; }
        

        [ForeignKey("TipoQuitacaoId")]
        public TipoQuitacao TipoQuitacao { get; set; }
        public long? TipoQuitacaoId { get; set; }


        public List<LancamentoQuitacao> LancamentoQuitacoes { get; set; }
    }
}
