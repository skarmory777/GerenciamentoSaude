using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.TiposGrupo;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos
{
    [Table("FatGrupo")]
    public class FaturamentoGrupo : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(100)]
        public override string Descricao { get; set; }

        [ForeignKey("TipoGrupoId")]
        public FaturamentoTipoGrupo TipoGrupo { get; set; }
        public long? TipoGrupoId { get; set; }

        //[ForeignKey("TabelaAnsId")]
        //public virtual TabelaAns TabelaAns { get; set; }// ainda nao foi criada
        //public long? TabelaAnsId { get; set; }

        public string CodTipoOutraDespesa { get; set; }

        public bool IsAtivo { get; set; }

        public bool IsOutraDespesa { get; set; }

        public bool IsObrigaMedico { get; set; }

        public bool IsTaxaUrgencia { get; set; }

        public bool IsPediatria { get; set; }

        public bool IsProcedimentoSerie { get; set; }

        public bool IsRequisicaoExame { get; set; }

        public bool IsPermiteRevisao { get; set; }

        public bool IsPrecoManual { get; set; }

        public bool IsAutorizacao { get; set; }

        public bool IsInternacao { get; set; }

        public bool IsAmbulatorio { get; set; }

        public bool IsCirurgia { get; set; }

        public bool IsPorte { get; set; }

        public bool IsConsultor { get; set; }

        public bool IsLaboratorio { get; set; }

        public bool IsPlantonista { get; set; }

        public bool IsOpme { get; set; }

        public bool IsExtraCaixa { get; set; }

        public bool IsLaudo { get; set; }

        public bool IsPacote { get; set; }

        public bool IsExame { get; set; }

        public bool IsEquipeMedicaVazia { get; set; }

        public bool IsTratamento { get; set; }

        public bool IsNaoAgrupaXml { get; set; }

        public bool IsDescontoCbhpm { get; set; }

        public bool IsTurno { get; set; }

        public bool IsMedicamento { get; set; }

        public bool IsOrteseProtese { get; set; }

        [ForeignKey("FaturamentoCodigoDespesaId")]
        public FaturamentoCodigoDespesa FaturamentoCodigoDespesa { get; set; }

        public long? FaturamentoCodigoDespesaId { get; set; }

        public long? OrdemAmbulatorio { get; set; }
        public long? OrdemInternacao { get; set; }
    }

}


