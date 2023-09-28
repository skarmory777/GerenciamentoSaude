using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Faturamentos.Grupos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Configuracoes.Empresas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Taxas
{
    [Table("FatTaxa")]
    public class FaturamentoTaxa : CamposPadraoCRUD
    {
        [StringLength(10)]
        public override string Codigo { get; set; }

        [StringLength(255)]
        public override string Descricao { get; set; }
        [Index("Fat_Idx_Nivel")]
        public long? Nivel { get; set; } = 1;
        [Index("Fat_Idx_DataInicio")]
        [DataType(DataType.DateTime)]
        public DateTime? DataInicio { get; set; }
        [Index("Fat_Idx_DataFim")]
        [DataType(DataType.DateTime)]
        public DateTime? DataFim { get; set; }

        public double Percentual { get; set; }
        [Index("Fat_Idx_IsAmbulatorio")]
        public bool IsAmbulatorio { get; set; }
        [Index("Fat_Idx_IsInternacao")]
        public bool IsInternacao { get; set; }
        [Index("Fat_Idx_IsIncideFilme")]
        public bool IsIncideFilme { get; set; }

        public bool IsIncidePorte { get; set; }
        public bool IsIncidePrecoItem { get; set; }

        public bool IsIncideManual { get; set; }
        public bool IsImplicita { get; set; }
        public bool IsTodosLocal { get; set; }
        public bool IsTodosTurno { get; set; }
        public bool IsTodosTipoLeito { get; set; }
        public bool IsTodosGrupo { get; set; }
        public bool IsTodosItem { get; set; }
        public bool IsTodosConvenio { get; set; }
        public bool IsTodosPlano { get; set; }

        public string LocalImpressao { get; set; }

        public long? ConvenioId { get; set; }

        [ForeignKey("ConvenioId")]
        public Convenio Convenio { get; set; }

        public long? EmpresaId { get; set; }

        [ForeignKey("EmpresaId")]
        public Empresa Empresa { get; set; }

        public long? LocalUtilizacaoId { get; set; }

        [ForeignKey("LocalUtilizacaoId")]
        public UnidadeOrganizacional LocalUtilizacao { get; set; }

        public long? FaturamentoGrupoId { get; set; }

        [ForeignKey("FaturamentoGrupoId")]
        public FaturamentoGrupo FaturamentoGrupo { get; set; }

        public long? TipoLeitoId { get; set; }

        //[ForeignKey("TipoLeitoId")]
        //public TipoLeito TipoLeito { get; set; }

        [ForeignKey("TipoAcomodacao"), Column("TipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacao TipoAcomodacao { get; set; }

        [ForeignKey("Item"), Column("ItemId")]
        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }

        public long? TurnoId { get; set; }

        [ForeignKey("TurnoId")]
        public Turno Turno { get; set; }

        public List<FaturamentoTaxaEmpresa> TaxaEmpresas { get; set; }

        public List<FaturamentoTaxaLocal> TaxaLocais { get; set; }

        public List<FaturamentoTaxaTurno> TaxaTurnos { get; set; }

        public List<FaturamentoTaxaTipoLeito> TaxaTiposLeitos { get; set; }

        public List<FaturamentoTaxaGrupo> TaxaGrupos { get; set; }

        public List<FaturamentoTaxaItem> TaxaItems { get; set; }
    }

    [Table("FatTaxaEmpresa")]
    public class FaturamentoTaxaEmpresa : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoTaxa"), Column("FatTaxaId")]
        public long? TaxaId { get; set; }
        public FaturamentoTaxa FaturamentoTaxa { get; set; }

        [ForeignKey("Empresa"), Column("SisEmpresaId")]
        public long? EmpresaId { get; set; }
        public Empresa Empresa { get; set; }
    }

    [Table("FatTaxaLocal")]
    public class FaturamentoTaxaLocal : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoTaxa"), Column("FatTaxaId")]
        public long? TaxaId { get; set; }
        public FaturamentoTaxa FaturamentoTaxa { get; set; }

        [ForeignKey("UnidadeOrganizacional"), Column("SisUnidadeOrganizacionalId")]
        public long? UnidadeOrganizacaionalId { get; set; }
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }
    }

    [Table("FatTaxaTurno")]
    public class FaturamentoTaxaTurno : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoTaxa"), Column("FatTaxaId")]
        public long? TaxaId { get; set; }
        public FaturamentoTaxa FaturamentoTaxa { get; set; }

        [ForeignKey("Turno"), Column("SisTurnoId")]
        public long? TurnoId { get; set; }
        public Turno Turno { get; set; }
    }

    [Table("FatTaxaTipoLeito")]
    public class FaturamentoTaxaTipoLeito : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoTaxa"), Column("FatTaxaId")]
        public long? TaxaId { get; set; }
        public FaturamentoTaxa FaturamentoTaxa { get; set; }

        //[ForeignKey("TipoLeito"), Column("TipoLeitoId")]
        //public long? TipoLeitoId { get; set; }
        //public TipoLeito TipoLeito { get; set; }

        [ForeignKey("TipoAcomodacao"), Column("TipoAcomodacaoId")]
        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacao TipoAcomodacao { get; set; }

    }

    [Table("FatTaxaGrupo")]
    public class FaturamentoTaxaGrupo : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoTaxa"), Column("FatTaxaId")]
        public long? TaxaId { get; set; }
        public FaturamentoTaxa FaturamentoTaxa { get; set; }

        [ForeignKey("FaturamentoGrupo"), Column("SisGrupoId")]
        public long? GrupoId { get; set; }
        public FaturamentoGrupo FaturamentoGrupo { get; set; }
    }


    [Table("FatTaxaItem")]
    public class FaturamentoTaxaItem : CamposPadraoCRUD
    {
        [ForeignKey("FaturamentoTaxa"), Column("FatTaxaId")]
        public long? TaxaId { get; set; }
        public FaturamentoTaxa FaturamentoTaxa { get; set; }

        [ForeignKey("Item"), Column("ItemId")]
        public long? ItemId { get; set; }
        public FaturamentoItem Item { get; set; }
    }

}


