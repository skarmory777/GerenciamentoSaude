using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Origens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssSolicitacaoExame")]
    public class SolicitacaoExame : CamposPadraoCRUD
    {
        [ForeignKey("Atendimento"), Column("AtendimentoId")]
        public long? AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }

        [Index("Ate_Idx_DataSolicitacao")]
        public DateTime DataSolicitacao { get; set; }

        [ForeignKey("Origem"), Column("SisOrigemId")]
        public long? OrigemId { get; set; }

        public Origem Origem { get; set; }

        [ForeignKey("Leito"), Column("SisLeitoId")]
        public long? LeitoId { get; set; }

        public Leito Leito { get; set; }

        public int Prioridade { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("MedicoSolicitante"), Column("SisMedicoSolicitanteId")]
        public long? MedicoSolicitanteId { get; set; }

        public Medico MedicoSolicitante { get; set; }

        public string Observacao { get; set; }

        [ForeignKey("Prescricao"), Column("AssPrescricaoId")]
        public long? PrescricaoId { get; set; }

        public PrescricaoMedica Prescricao { get; set; }

        public List<SolicitacaoExameItem> SolicitacaoExameItens { get; set; }

        public string Justificativa { get; set; }
        
        public long? PendenciaUserId { get; set; }

        [Index("Ate_Idx_PendenciaDateTime")]
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }

        public SolicitacaoExame()
        {
            DataSolicitacao = DateTime.Now;
        }
    }
}
