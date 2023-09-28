using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens
{
    [Table("LauMovimentoItem")]
    public class LaudoMovimentoItem : CamposPadraoCRUD
    {
        [Column("LauMovimentoId"), ForeignKey("LaudoMovimento")]
        public long? LaudoMovimentoId { get; set; }

        [Column("FatItemId"), ForeignKey("FaturamentoItem")]
        public long FaturamentoItemId { get; set; }

        [Column("AssSolicitacaoExameItemId"), ForeignKey("SolicitacaoExameItem")]
        public long? SolicitacaoExameItemId { get; set; }

        public LaudoMovimento LaudoMovimento { get; set; }
        public FaturamentoItem FaturamentoItem { get; set; }
        public SolicitacaoExameItem SolicitacaoExameItem { get; set; }

        public long? TecnicoId { get; set; }

        public string Parecer { get; set; }
        public long? UsuarioParecerId { get; set; }
        [Index("Lau_Idx_ParecerData")]
        public DateTime? ParecerData { get; set; }
        public string Laudo { get; set; }
        public long? UsuarioLaudoId { get; set; }
        [Index("Lau_Idx_LaudoData")]
        public DateTime? LaudoData { get; set; }
        public string ConcordanciaLaudo { get; set; }
        public string JustificativaConcoLaudo { get; set; }
        public string Revisao { get; set; }
        public long? UsuarioRevisaoId { get; set; }
        [Index("Lau_Idx_RevisaoData")]
        public DateTime? RevisaoData { get; set; }
        public string Retificacao { get; set; }
        public long? UsuarioRetificacaoId { get; set; }
        [Index("Lau_Idx_RetificacaoData")]
        public DateTime? RetificacaoData { get; set; }
        public int Status { get; set; }

        [ForeignKey("FaturamentoContaItem"), Column("FaturamentocontaItemId")]
        public long? FaturamentocontaItemId { get; set; }
        public FaturamentoContaItem FaturamentoContaItem { get; set; }

        public bool IsIndicativo { get; set; }
        public bool IsSolicitacaoRevisao { get; set; }
        public string ComentarioLaudo { get; set; }
        public string JustificativaContraste { get; set; }
        public string MotivoDiscordancia { get; set; }

        public string AccessNumber { get; set; }


    }
}
