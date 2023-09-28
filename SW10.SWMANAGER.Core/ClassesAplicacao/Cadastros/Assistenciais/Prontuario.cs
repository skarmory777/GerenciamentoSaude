using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.PrescricoesLogs;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.ProfissionaisSaude;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.GeradorFormularios;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Operacoes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais
{
    [Table("AssProntuario")]
    public class Prontuario : CamposPadraoCRUD, IDescricao
    {
        [Index("Ass_Idx_DataAdmissao")]
        public DateTime DataAdmissao { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long UnidadeOrganizacionalId { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long AtendimentoId { get; set; }

        public Atendimento Atendimento { get; set; }

        [ForeignKey("ProfissionalSaude"), Column("SisProfissionalSaudeId")]
        public long? ProfissionalSaudeId { get; set; }

        public ProfissionalSaude ProfissionalSaude { get; set; }

        public string Observacao { get; set; }
        [ForeignKey("FormResposta"), Column("SisFormRespostaId")]
        public long? FormRespostaId { get; set; }

        public FormResposta FormResposta { get; set; }

        [ForeignKey("Operacao"), Column("SisOperacaoId")]
        public long? OperacaoId { get; set; }

        public Operacao Operacao { get; set; }

        [ForeignKey("ProntuarioPrincipal"), Column("AssProntuarioPrincipalId")]
        public long? ProntuarioPrincipalId { get; set; }

        public Prontuario ProntuarioPrincipal { get; set; }

        public ICollection<Prontuario> SubProntuarios { get; set; }

        public ICollection<ProntuarioLog> ProntuarioLogs { get; set; }

        public bool EstaInativo { get; set; }
        
        public long? InativacaoUserId { get; set; }
        [Index("Ass_Idx_InativacaoData")]
        public DateTime? InativacaoData { get; set; }

        public string InativacaoJustificativa { get; set; }

        public long? AtivacaoUserId { get; set; }
        [Index("Ass_Idx_AtivacaoData")]
        public DateTime? AtivacaoData { get; set; }

        public string AtivacaoJustificativa { get; set; }

        [ForeignKey("Leito"), Column("AteLeitoId")]
        public long? LeitoId { get; set; }
        public Leito Leito { get; set; }
    }
}
