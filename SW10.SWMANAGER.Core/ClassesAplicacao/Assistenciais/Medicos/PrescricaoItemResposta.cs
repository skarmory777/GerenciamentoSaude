using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Divisoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.FormasAplicacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.Frequencias;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesItens;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.PrescricoesStatus;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Assistenciais.Prescricoes.VelocidadesInfusao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos
{
    [Table("AssPrescricaoItemResposta")]
    public class PrescricaoItemResposta : CamposPadraoCRUD
    {
        public decimal? Quantidade { get; set; }

        [ForeignKey("Unidade"), Column("EstUnidadeId")]
        public long? UnidadeId { get; set; }

        [ForeignKey("VelocidadeInfusao"), Column("AssVelocidadeInfusaoId")]
        public long? VelocidadeInfusaoId { get; set; }

        [ForeignKey("FormaAplicacao"), Column("AssFormaAplicacaoId")]
        public long? FormaAplicacaoId { get; set; }

        [ForeignKey("Frequencia"), Column("AssFrequenciaId")]
        public long? FrequenciaId { get; set; }

        [Index("Ass_Idx_IsSeNecessario")]
        public bool IsSeNecessario { get; set; }

        [Index("Ass_Idx_IsUrgente")]
        public bool IsUrgente { get; set; }

        public bool IsDias { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacionalId { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoId")]
        public long? MedicoId { get; set; }

        [Index("Ass_Idx_DataInicial")]
        public DateTime? DataInicial { get; set; }

        public double DiaAtual { get { return DataInicial.HasValue ? DateTime.Now.Subtract(DataInicial.Value).Days : 0; } }

        public int? TotalDias { get; set; }

        public string Observacao { get; set; }

        public VelocidadeInfusao VelocidadeInfusao { get; set; }

        public FormaAplicacao FormaAplicacao { get; set; }

        public Frequencia Frequencia { get; set; }

        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        public Medico Medico { get; set; }

        public Unidade Unidade { get; set; }

        [ForeignKey("PrescricaoItem"), Column("AssPrescricaoItemId")]
        public long? PrescricaoItemId { get; set; }

        public PrescricaoItem PrescricaoItem { get; set; }

        [ForeignKey("Divisao"), Column("AssDivisaoId")]
        public long? DivisaoId { get; set; }

        public Divisao Divisao { get; set; }

        [ForeignKey("PrescricaoMedica"), Column("AssPrescricaoMedicaId")]
        public long? PrescricaoMedicaId { get; set; }

        public PrescricaoMedica PrescricaoMedica { get; set; }

        [ForeignKey("PrescricaoItemStatus"), Column("AssPrescricaoItemStatusId")]
        public long? PrescricaoItemStatusId { get; set; }
        public PrescricaoStatus PrescricaoItemStatus { get; set; }

        public long? AprovadoUserId { get; set; }

        [Index("Ass_Idx_DataAprovado")]
        public DateTime? DataAprovado { get; set; }

        public long? LiberadoUserId { get; set; }

        [Index("Ass_Idx_DataLiberado")]
        public DateTime? DataLiberado { get; set; }

        [Index("Ass_Idx_IsAcrescimo")]
        public bool IsAcrescimo { get; set; }

        public long? AcrescimoUserId { get; set; }

        [Index("Ass_Idx_DataAcrescimo")]
        public DateTime? DataAcrescimo { get; set; }

        public bool IsSuspenso { get; set; }

        public long? SuspensoUserId { get; set; }

        [Index("Ass_Idx_DataSuspenso")]
        public DateTime? DataSuspenso { get; set; }

        public long? ReativadoUserId { get; set; }

        [Index("Ass_Idx_DataReativado")]
        public DateTime? DataReativado { get; set; }

        public long? DiluenteId { get; set; }

        [ForeignKey("DiluenteId"), Column("AssDiluenteId")]
        public PrescricaoItem Diluente { get; set; }

        public double? VolumeDiluente { get; set; }

        public bool DoseUnica { get; set; }

        [Index("Ass_Idx_DataAgrupamento")]
        public DateTime DataAgrupamento { get; set; }
        
        public string ObsFrequencia { get; set; }

        public string JustificativaBloqueioDosagemAceitavel { get; set; }
        public long? JustificativaBloqueioId { get; set; }

    }
}
