using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes
{
    [Table("AteAutorizacaoProcedimentoItem")]
    public class AutorizacaoProcedimentoItem : CamposPadraoCRUD
    {
        public long? FaturamentoItemId { get; set; }
        public string Senha { get; set; }

        [Index("Ate_Idx_DataAutorizacao")]
        public DateTime? DataAutorizacao { get; set; }
        [Index("Ate_Idx_IsOrtese")]
        public bool IsOrtese { get; set; }
        public string AutorizadoPor { get; set; }
        public int? QuantidadeSolicitada { get; set; }
        public int? QuantidadeAutorizada { get; set; }
        public long? StatusId { get; set; }
        public string Observacao { get; set; }

        public long AutorizacaoProcedimentoId { get; set; }

        [ForeignKey("AutorizacaoProcedimentoId")]
        public AutorizacaoProcedimento AutorizacaoProcedimento { get; set; }

        [ForeignKey("FaturamentoItemId")]
        public FaturamentoItem FaturamentoItem { get; set; }

        [ForeignKey("StatusId")]
        public StatusSolicitacaoProcedimento StatusSolicitacaoProcedimento { get; set; }

    }
}
