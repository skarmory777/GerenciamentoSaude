using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Atendimentos.Leitos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.CentrosCustos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Convenios;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Medicos;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.Terceirizados;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.TiposAcomodacao;
using SW10.SWMANAGER.ClassesAplicacao.Cadastros.UnidadesOrganizacionais;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabResultado")]
    public class Resultado : CamposPadraoCRUD
    {
        public long? TecnicoId { get; set; }
        public long? FaturamentoContaId { get; set; }
        public long? ResponsavelId { get; set; }
        public long? UsuarioConferidoId { get; set; }//int] NULL,
        public long? UsuarioDigitadoId { get; set; }//int] NULL,
        public long? UsuarioEntregaId { get; set; }//int] NULL,
        public long? UsuarioCienteId { get; set; }//int] NULL,
        public long? TecnicoColetaId { get; set; }//int] NULL,
        public long? LeitoAtualId { get; set; }//int] NULL,
        public long? LocalAtualId { get; set; }//int] NULL,
        public long? RotinaId { get; set; }//int] NULL,
        public long? RequisicaoMovId { get; set; }//int] NULL,

        [ForeignKey("MedicoSolicitante"), Column("SisMedicoSolicitanteId")]
        public long? MedicoSolicitanteId { get; set; }
        [ForeignKey("Atendimento"), Column("AteAtendimentoId")]
        public long? AtendimentoId { get; set; }

        public Medico MedicoSolicitante { get; set; }
        public Atendimento Atendimento { get; set; }

        [ForeignKey("TecnicoId")]
        public Tecnico Tecnico { get; set; }

        [ForeignKey("FaturamentoContaId")]
        public FaturamentoConta FaturamentoConta { get; set; }

        [ForeignKey("ResponsavelId")]
        public Tecnico Responsavel { get; set; }

        [ForeignKey("UsuarioConferidoId")]
        public User UsuarioConferido { get; set; }
        [ForeignKey("UsuarioDigitadoId")]
        public User UsuarioDigitado { get; set; }
        [ForeignKey("UsuarioEntregaId")]
        public User UsuarioEntrega { get; set; }
        [ForeignKey("UsuarioCienteId")]
        public User UsuarioCiente { get; set; }

        [ForeignKey("TecnicoColetaId")]
        public Tecnico TecnicoColeta { get; set; }

        [ForeignKey("LeitoAtualId")]
        public Leito LeitoAtual { get; set; }

        [ForeignKey("LocalAtualId")]
        public UnidadeOrganizacional LocalAtual { get; set; }

        public bool IsRn { get; set; }//dbo].[TBitControl] NULL,

        public bool IsEmail { get; set; }//dbo].[TBitControl] NULL,

        public bool IsEmergencia { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsUrgente { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsAvisoLab { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsAvisoMed { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsVisualiza { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsRotina { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsTransferencia { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsCiente { get; set; }//dbo].[TBitControl] NOT NULL,

        public string Numero { get; set; }
        public long Nic { get; set; }
        [Index("Lab_Idx_DataColeta")]
        public DateTime? DataColeta { get; set; }
        public long? SexoRnId { get; set; }//dbo].[TSexo] NULL,	    
        [Index("Lab_Idx_DataDigitado")]
        public DateTime? DataDigitado { get; set; }//dbo].[TDateTime] NULL,	    
        [Index("Lab_Idx_DataConferido")] 
        public DateTime? DataConferido { get; set; }//dbo].[TDateTime] NULL,	    
        [Index("Lab_Idx_DataEnvioEmail")] 
        public DateTime? DataEnvioEmail { get; set; }//datetime] NULL,
        [Index("Lab_Idx_DataEntregaExame")] 
        public DateTime? DataEntregaExame { get; set; }//datetime] NULL,	    
        public string ObsEntrega { get; set; }//varchar](100) NULL,
        public string PessoaEntrega { get; set; }//varchar](50) NULL,

        [Index("Lab_Idx_DataPrevEntregaExame")] 
        public DateTime? DataPrevEntregaExame { get; set; }//dbo].[TDateTime] NULL,
        public string Gemelar { get; set; }//varchar](5) NULL,	    
        [Index("Lab_Idx_DataTecnico")] 
        public DateTime? DataTecnico { get; set; }//dbo].[TDateTime] NULL,
        [Index("Lab_Idx_DataUsuarioCiente")] 
        public DateTime? DataUsuarioCiente { get; set; }//dbo].[TDateTime] NULL,
        public string Peso { get; set; }//varchar](10) NULL,
        public string Altura { get; set; }//varchar](10) NULL,
        public string Remedio { get; set; }//varchar](500) NULL,


        public long? ConvenioId { get; set; }
        public Convenio Convenio { get; set; }

        public long? CentroCustoId { get; set; }
        public CentroCusto CentroCusto { get; set; }

        public long? TipoAcomodacaoId { get; set; }
        public TipoAcomodacao TipoAcomodacao { get; set; }

        public long? TurnoId { get; set; }
        public Turno Turno { get; set; }

        public string NomeMedicoSolicitante { get; set; }
        public string CRMSolicitante { get; set; }

        [ForeignKey("ResultadoStatus"), Column("LabResultadoStatusId")]
        public long? ResultadoStatusId { get; set; }
        public ResultadoStatus ResultadoStatus { get; set; }
        

        [ForeignKey("AutorizacaoProcedimento"), Column("AteAutorizacaoProcedimentoId")]
        public long? AutorizacaoProcedimentoId { get; set; }
        public AutorizacaoProcedimento AutorizacaoProcedimento { get; set; }

        [ForeignKey("LocalUtilizacao"), Column("SisLocalUtilizacaoId")]
        public long? LocalUtilizacaoId { get; set; }
        public UnidadeOrganizacional LocalUtilizacao { get; set; }

        [ForeignKey("Terceirizado"), Column("SisTerceirizadoId")]
        public long? TerceirizadoId { get; set; }
        public Terceirizado Terceirizado { get; set; }
        
        [ForeignKey("SolicitacaoExame"), Column("SolicitacaoExameId")]
        public long? SolicitacaoExameId { get; set; }
        public SolicitacaoExame SolicitacaoExame { get; set; }
        public bool IsPendencia { get; set; }
        public long? PendenciaUserId { get; set; }
        [Index("Lab_Idx_PendenciaDateTime")]
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }
    }
}
