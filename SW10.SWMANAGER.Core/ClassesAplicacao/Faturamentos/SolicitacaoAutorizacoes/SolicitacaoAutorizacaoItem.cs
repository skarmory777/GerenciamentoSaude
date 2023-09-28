using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SolicitacaoAutorizacoes
{
    [Table("FatSolicitacaoAutorizacaoItem")]
    public class SolicitacaoAutorizacaoItem : CamposPadraoCRUD
    {
        public long? SolicitacaoAutorizacaoStatusId { get; set; }

        public SolicitacaoAutorizacaoStatus SolicitacaoAutorizacaoStatus { get; set; }
        public long? SolicitacaoAutorizacaoId { get; set; }

        public SolicitacaoAutorizacao SolicitacaoAutorizacao { get; set; }

        public long? AtendimentoId { get; set; }

        public long? MedicoId { get; set; }

        public long? PrescricaoId { get; set; }

        public PrescricaoMedica Prescricao { get; set; }

        public Atendimento Atendimento { get; set; }

        public Medico Medico { get; set; }

        public long? FaturamentoItemId { get; set; }
        public FaturamentoItemDto FaturamentoItem { get; set; }
        [Index("Fat_Idx_DataSolicitacao")]
        public DateTime DataSolicitacao { get; set; }
        [Index("Fat_Idx_DataMaximaTempoProvavel")]
        public DateTime DataMaximaTempoProvavel { get; set; }

        public int TempoProvavelUso { get; set; }

        public string Resultados { get; set; }

        public string Dose { get; set; }
    }
}
