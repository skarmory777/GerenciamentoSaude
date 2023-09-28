using Abp.Organizations;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.UnidadesInternacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Suprimentos.Estoques;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais
{
    [Table("SisUnidadeOrganizacional")]

    public class UnidadeOrganizacional : CamposPadraoCRUD
    {
        [StringLength(255)]
        public string Localizacao { get; set; }
        [Index("Sis_Idx_IsAtivo")]
        public bool IsAtivo { get; set; }

        public bool ControlaAlta { get; set; }

        [Index("Sis_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }
        [Index("Sis_Idx_IsAmbulatorioEmergencia")]
        public bool IsAmbulatorioEmergencia { get; set; }

        public bool IsHospitalDia { get; set; }

        public bool IsSetorExames { get; set; }

        [Index("Sis_Idx_IsEstoque")]
        public bool IsEstoque { get; set; }

        public bool IsLocalUtilizacao { get; set; }

        public string HoraInicioPrescricao { get; set; }

        //TODO: unidadeInternacaoTipo existe? esta sendo usada?
        [ForeignKey("UnidadeInternacaoTipo"), Column("AteUnidadeInternacaoTipoId")]
        public long? UnidadeInternacaoTipoId { get; set; }

        public UnidadeInternacaoTipo UnidadeInternacaoTipo { get; set; }

        [ForeignKey("OrganizationUnit"), Column("SisOrganizationUnitId")]
        public long OrganizationUnitId { get; set; }

        public OrganizationUnit OrganizationUnit { get; set; }

        [ForeignKey("CentroCusto"), Column("CentroCustoId")]
        public long? CentroCustoId { get; set; }

        public CentroCusto CentroCusto { get; set; }

        [ForeignKey("Estoque"), Column("EstEstoqueId")]
        public long? EstoqueId { get; set; }

        public Estoque Estoque { get; set; }

        public int? ImportaLocalUtilizacaoId { get; set; }

        [Index("Sis_Idx_IsControlaLeito")]
        public bool IsControlaLeito { get; set; }
    }
}
