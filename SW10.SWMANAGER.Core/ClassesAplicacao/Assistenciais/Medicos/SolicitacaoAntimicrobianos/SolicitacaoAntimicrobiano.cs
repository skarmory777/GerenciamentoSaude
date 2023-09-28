using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssSolicitacaoAntimicrobianos")]
    public class SolicitacaoAntimicrobiano : CamposPadraoCRUD
    {
        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long? AtendimentoId { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }

        [ForeignKey("PrescricaoItem"), Column("AssPrescricaoItemId")]
        public long? PrescricaoItemId { get; set; }
        
        
        [ForeignKey("Frequencia"), Column("AssFrequenciaId")]
        public long? FrequenciaId { get; set; }
        
        public Frequencia Frequencia { get; set; }
        
        [ForeignKey("Unidade")]
        public long? UnidadeId { get; set; }
        
        public Unidade Unidade { get; set; }
        
        [ForeignKey("FormaAplicacao"), Column("AssFormaAplicacaoId")]
        public long? FormaAplicacaoId { get; set; }
        
        public FormaAplicacao FormaAplicacao { get; set; }
        
        [ForeignKey("VelocidadeInfusao"), Column("AssVelocidadeInfusaoId")]
        public long? VelocidadeInfusaoId { get; set; }
        
        public VelocidadeInfusao VelocidadeInfusao { get; set; }
        
        public decimal? Qtd { get; set; }
        

        public Atendimento Atendimento { get; set; }

        public Medico Medico { get; set; }

        public PrescricaoItem PrescricaoItem { get; set; }

        [Index("Ate_Idx_DataSolicitacao")]
        public DateTime DataSolicitacao { get; set; }

        [Index("Ate_Idx_DataMaximaTempoProvavel")]
        public DateTime DataMaximaTempoProvavel { get; set; }

        [ForeignKey("PrescricaoItemResposta"), Column("AssPrescricaoItemRespostaId")]
        public long? PrescricaoItemRespostaId { get; set; }
        public PrescricaoItemResposta PrescricaoItemResposta { get; set; }

        public int TempoProvavelUso { get; set; }

        public string TipoInfeccao { get; set; }

        public string TipoCultura { get; set; }

        public string OutrasIndicacoes { get; set; }

        public string OutrosResultados { get; set; }

        public ICollection<SolicitacaoAntimicrobianosIndicacao> SolicitacaoAntimicrobianosIndicacoes { get; set; }

        public ICollection<SolicitacaoAntimicrobianosCulturas> SolicitacaoAntimicrobianosCulturas { get; set; }
    }
}
