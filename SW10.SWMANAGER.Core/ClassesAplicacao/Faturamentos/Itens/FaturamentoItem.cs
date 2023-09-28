using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.BrasItens;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Kits;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.SubGrupos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens
{
    [Table("FatItem")]
    public class FaturamentoItem : CamposPadraoCRUD
    {
        [StringLength(20)]
        public override string Codigo { get; set; }

        [StringLength(100)]
        public override string Descricao { get; set; }

        [ForeignKey("BrasItemId")]
        public FaturamentoBrasItem BrasItem { get; set; }
        public long? BrasItemId { get; set; }

        [ForeignKey("GrupoId")]
        public FaturamentoGrupo Grupo { get; set; }
        public long? GrupoId { get; set; }

        [ForeignKey("SubGrupoId")]
        public FaturamentoSubGrupo SubGrupo { get; set; }
        public long? SubGrupoId { get; set; }

        [ForeignKey("LaudoGrupo"), Column("LauGrupoId")]
        public long? LaudoGrupoId { get; set; }
        public LaudoGrupo LaudoGrupo { get; set; }

        [StringLength(255)]
        public string DescricaoTuss { get; set; }

        [StringLength(255)]
        public string Observacao { get; set; }

        [StringLength(20)]
        public string CodAmb { get; set; }

        [StringLength(20)]
        public string CodTuss { get; set; }

        [StringLength(20)]
        public string CodCbhpm { get; set; }

        public float DivideBrasindice { get; set; }

        public string Referencia { get; set; }

        public string ReferenciaSihSus { get; set; }

        public int Sexo { get; set; }

        public int QtdLaudo { get; set; }

        public int TipoLaudo { get; set; }

        public int DuracaoMinima { get; set; }
        [Index("Fat_Idx_IsAtivo")]
        public bool IsAtivo { get; set; }

        public bool IsObrigaMedico { get; set; }

        public bool IsTaxaUrgencia { get; set; }

        public bool IsPediatria { get; set; }

        public bool IsProcedimentoSerie { get; set; }

        public bool IsRequisicaoExame { get; set; }

        public bool IsPermiteRevisao { get; set; }

        public bool IsPrecoManual { get; set; }

        public bool IsAutorizacao { get; set; }
        [Index("Fat_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }
        [Index("Fat_Idx_IsAmbulatorio")]
        public bool IsAmbulatorio { get; set; }

        public bool IsCirurgia { get; set; }

        public bool IsPorte { get; set; }

        public bool IsConsultor { get; set; }
        [Index("Fat_Idx_IsLaboratorio")]
        public bool IsLaboratorio { get; set; }

        public bool IsPlantonista { get; set; }

        public bool IsOpme { get; set; }

        public bool IsExtraCaixa { get; set; }

        public bool IsLaudo { get; set; }

        public List<FaturamentoKitItem> FatKits { get; set; }

        #region Exame

        public bool IsExameSimples { get; set; }
        public bool IsPeso { get; set; }
        public bool IsTesta100 { get; set; }
        public bool IsAltura { get; set; }
        public bool IsCor { get; set; }
        public bool IsMestruacao { get; set; }
        public bool IsNacionalidade { get; set; }
        public bool IsNaturalidade { get; set; }
        public bool IsImpReferencia { get; set; }
        [Index("Fat_Idx_IsCultura")]
        public bool IsCultura { get; set; }
        public bool IsPendente { get; set; }
        public bool IsRepete { get; set; }
        public bool IsLibera { get; set; }
        public string Mneumonico { get; set; }
        public int? OrdemImp { get; set; }
        public int? Prazo { get; set; }
        public byte[] Interpretacao { get; set; }
        public byte[] Extra1 { get; set; }
        public byte[] Extra2 { get; set; }
        public int? QtdFatura { get; set; }
        public string MapaExame { get; set; }
        public int? OrdemResul { get; set; }
        public int? OrdemResumo { get; set; }
        public int? OrdemMapaResultado { get; set; }
        public long? EquipamentoId { get; set; }
        public long? ExameIncluiId { get; set; }
        public long? SetorId { get; set; }
        public long? MaterialId { get; set; }
        public long? MetodoId { get; set; }
        public long? UnidadeId { get; set; }
        public long? FormataId { get; set; }
        public long? MapaId { get; set; }

        [ForeignKey("EquipamentoId")]
        public Equipamento Equipamento { get; set; }

        [ForeignKey("ExameIncluiId")]
        public FaturamentoItem ExameInclui { get; set; }

        [ForeignKey("SetorId")]
        public Setor Setor { get; set; }

        [ForeignKey("MaterialId")]
        public Material Material { get; set; }

        [ForeignKey("MetodoId")]
        public Metodo Metodo { get; set; }

        [ForeignKey("UnidadeId")]
        public LaboratorioUnidade Unidade { get; set; }

        [ForeignKey("FormataId")]
        public Formata Formata { get; set; }

        [ForeignKey("MapaId")]
        public Mapa Mapa { get; set; }

        #endregion

        #region Agendamento

        public bool IsAgendaConsulta { get; set; }
        public bool IsAgendaCirurgia { get; set; }
        public bool IsAgendaExame { get; set; }
        public bool IsAgendaMaterial { get; set; }
        public int QuantidadeMinutos { get; set; }

        #endregion

    }

}


