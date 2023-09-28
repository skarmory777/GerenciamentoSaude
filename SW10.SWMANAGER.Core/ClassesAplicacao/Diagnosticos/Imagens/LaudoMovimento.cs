using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SW10.SWMANAGER.ClassesAplicacao.Diagnosticos.Imagens
{
    [Table("LauMovimento")]
    public class LaudoMovimento : CamposPadraoCRUD
    {
        #region Key/Index Property
        [Index("IX_LauMovimento_Codigo")]
        [StringLength(10)]
        public override string Codigo { get; set; }
        #endregion

        #region ForeignKey Property
        [Column("AtendimentoId"), ForeignKey("Atendimento")]
        public long AtendimentoId { get; set; }

        [Column("LauMovimentoStatusId"), ForeignKey("LaudoMovimentoStatus")]
        public long LaudoMovimentoStatusId { get; set; }

        [Column("ConvenioId"), ForeignKey("Convenio")]
        public long? ConvenioId { get; set; }

        [Column("LeitoId"), ForeignKey("Leito")]
        public long? LeitoId { get; set; }

        [Column("UnidadeOrganizacionalId"), ForeignKey("UnidadeOrganizacional")]
        public long? UnidadeOrganizacionalId { get; set; }

        [ForeignKey("UnidadeOrganizacionalId")]
        public UnidadeOrganizacional UnidadeOrganizacional { get; set; }

        [Column("CentroCustoId"), ForeignKey("CentroCusto")]
        public long? CentroCustoId { get; set; }

        [ForeignKey("CentroCustoId")]
        public CentroCusto CentroCusto { get; set; }

        [Column("TipoAcomodacaoId"), ForeignKey("TipoAcomodacao")]
        public long? TipoAcomodacaoId { get; set; }

        [ForeignKey("TipoAcomodacaoId")]
        public TipoAcomodacao TipoAcomodacao { get; set; }

        [Column("TurnoId"), ForeignKey("Turno")]
        public long? TurnoId { get; set; }

        [ForeignKey("TurnoId")]
        public Turno Turno { get; set; }

        [ForeignKey("Medico"), Column("SisMedicoSolicitanteId")]
        public long? MedicoSolicitanteId { get; set; }
        public Medico Medico { get; set; }

        [ForeignKey("Tecnico"), Column("LabTecnicoId")]
        public long? TecnicoId { get; set; }
        public Tecnico Tecnico { get; set; }

        public string MedicoSolicitante { get; set; }
        public string Crm { get; set; }

        [ForeignKey("SolicitacaoExameItem")]
        public long? SolicitacaoExameItemId { get; set; }

        public SolicitacaoExameItem SolicitacaoExameItem { get; set; }
        #endregion

        #region Foreign Property
        public Atendimento Atendimento { get; set; }
        public LaudoMovimentoStatus LaudoMovimentoStatus { get; set; }
        public Convenio Convenio { get; set; }
        public Leito Leito { get; set; }
        public List<LaudoMovimentoItem> LaudoMovimentoItens { get; set; }
        #endregion

        #region Property
        public bool IsContraste { get; set; }
        public string QtdeConstraste { get; set; }
        public string Obs { get; set; }
        [Index("Lau_Idx_DataRegistro")]
        public DateTime DataRegistro { get; set; }

        public int? VolumeContrasteTotal { get; set; }
        public int? VolumeContrasteVenoso { get; set; }
        public int? VolumeContrasteOral { get; set; }
        public int? VolumeContrasteRetal { get; set; }
        public bool IsIonico { get; set; }
        public bool IsBombaInsufora { get; set; }
        public bool IsContrasteVenoso { get; set; }
        public bool IsContrasteOral { get; set; }
        public bool IsContrasteRetal { get; set; }

        public string LoteContraste { get; set; }



        #endregion


    }
}
