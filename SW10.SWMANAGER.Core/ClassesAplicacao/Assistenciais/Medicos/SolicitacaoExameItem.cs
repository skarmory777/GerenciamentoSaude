using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssSolicitacaoExameItem")]
    public class SolicitacaoExameItem : CamposPadraoCRUD
    {
        [ForeignKey("Solicitacao"), Column("AssSolicitacaoExameId")]
        public long SolicitacaoExameId { get; set; }

        public SolicitacaoExame Solicitacao { get; set; }

        [ForeignKey("FaturamentoItem"), Column("FatItemId")]
        public long? FaturamentoItemId { get; set; }

        public FaturamentoItem FaturamentoItem { get; set; }

        public string GuiaNumero { get; set; }

        [Index("Ate_Idx_DataValidade")]
        public DateTime? DataValidade { get; set; }

        public string SenhaNumero { get; set; }

        [ForeignKey("Material"), Column("LabMaterialId")]
        public long? MaterialId { get; set; }

        public Material Material { get; set; }

        public string Justificativa { get; set; }

        [ForeignKey("KitExame"), Column("LabKitExameId")]
        public long? KitExameId { get; set; }

        public KitExame KitExame { get; set; }

        [ForeignKey("StatusSolicitacaoExameItem"), Column("StatusSolicitacaoExameItemId")]
        public long? StatusSolicitacaoExameItemId { get; set; }

        public StatusSolicitacaoExameItem StatusSolicitacaoExameItem { get; set; }


        [ForeignKey("PrescricaoItemResposta")]
        public long? PrescricaoItemRespostaId { get; set; }

        public PrescricaoItemResposta PrescricaoItemResposta { get; set; }

        public string AccessNumber { get; set; }

        [Index("Ate_Idx_IsPendencia")]
        public bool IsPendencia { get; set; }
        public long? PendenciaUserId { get; set; }

        [Index("Ate_Idx_PendenciaDateTime")]
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }
    }
}
