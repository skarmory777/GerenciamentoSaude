using SW10.SWMANAGER.Authorization.Users;
using SW10.SWMANAGER.ClassesAplicacao.Atendimentos.CentralAutorizacoes;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Contas;
using SW10.SWMANAGER.ClassesAplicacao.Faturamentos.Itens;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using SW10.SWMANAGER.ClassesAplicacao.Assistenciais.Medicos;

namespace SW10.SWMANAGER.ClassesAplicacao.Cadastros.Laboratorios
{
    [Table("LabResultadoExame")]
    public class ResultadoExame : CamposPadraoCRUD
    {

        [ForeignKey("Formata"), Column("LabFormataId")]
        public long? FormataId { get; set; }//int] NULL,

        [ForeignKey("FaturamentoItem"), Column("LabFaturamentoItemId")]
        public long? FaturamentoItemId { get; set; }//int] NULL,

        [ForeignKey("Resultado"), Column("LabResultadoId")]
        public long? ResultadoId { get; set; }//int] NULL,

        [ForeignKey("FaturamentoContaItem"), Column("FatContaItemId")]
        public long? FaturamentoContaItemId { get; set; }//int] NULL,

        [ForeignKey("KitExame"), Column("LabKitExameId")]
        public long? KitExameId { get; set; }//int] NULL,

        [ForeignKey("UsuarioIncluidoExame"), Column("SisUsuarioIncluidoId")]
        public long? UsuarioIncluidoExameId { get; set; }//int] NULL,

        [ForeignKey("UsuarioConferidoExame"), Column("SisUsuarioConferidoId")]
        public long? UsuarioConferidoExameId { get; set; }//int] NULL,

        [ForeignKey("UsuarioDigitadoExame"), Column("SisUsuarioDigitadoid")]
        public long? UsuarioDigitadoExameId { get; set; }//int] NULL,

        [ForeignKey("UsuarioPendenteExame"), Column("SisUsuarioPendenteExameId")]
        public long? UsuarioPendenteExameId { get; set; }//int] NULL,

        [ForeignKey("UsuarioImpressoExame"), Column("SisUsuarioImpressoExameId")]
        public long? UsuarioImpressoExameId { get; set; }//int] NULL,

        [ForeignKey("Material"), Column("LabMaterialId")]
        public long? MaterialId { get; set; }//int] NULL,

        [ForeignKey("UsuarioCienteExame"), Column("SisUsuarioCienteExameId")]
        public long? UsuarioCienteExameId { get; set; }//int] NULL,

        [ForeignKey("UsuarioImpSolicita"), Column("SisUsuarioImpSolicitaId")]
        public long? UsuarioImpSolicitaId { get; set; }//int] NULL,

        [ForeignKey("UsuarioAlteradoExame"), Column("SisUsuarioAlteradoExameId")]
        public long? UsuarioAlteradoExameId { get; set; }//int] NULL,

        [ForeignKey("Tabela"), Column("LabTabelaId")]
        public long? TabelaId { get; set; }

        public Formata Formata { get; set; }
        
        
        public User UsuarioIncluidoExame { get; set; }
        public User UsuarioConferidoExame { get; set; }
        public User UsuarioDigitadoExame { get; set; }
        public User UsuarioPendenteExame { get; set; }
        public User UsuarioImpressoExame { get; set; }
        public User UsuarioCienteExame { get; set; }
        public User UsuarioImpSolicita { get; set; }
        public User UsuarioAlteradoExame { get; set; }
        public FaturamentoItem FaturamentoItem { get; set; }
        public Resultado Resultado { get; set; }
        public FaturamentoContaItem FaturamentoContaItem { get; set; }
        public KitExame KitExame { get; set; }
        public Material Material { get; set; }
        public Tabela Tabela { get; set; }

        public bool IsImprime { get; set; }//dbo].[TBitControl] NULL,

        public bool IsSigiloso { get; set; }//dbo].[TBitControl] NULL,

        public bool IsSergioFranco { get; set; }//dbo].[TBitControl] NOT NULL,

        public bool IsCienteExame { get; set; }//dbo].[TBitControl] NOT NULL,

        public int Seq { get; set; }//int] NULL,
        
        public DateTime? DataColetaBaixa { get; set; }
        
        public User UsuarioColetaBaixa { get; set; }
        
        [ForeignKey("UsuarioColetaBaixa"), Column("UsuarioColetaBaixaId")]
        public long? UsuarioColetaBaixaId { get; set; }
        
        public Tecnico TecnicoColeta { get; set; }
        [ForeignKey("TecnicoColeta"), Column("TecnicoColetaId")]
        public long? TecnicoColetaId { get; set; }
        
        public DateTime? DataInclusao { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataAlteracao { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataExclusao { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataConferidoExame { get; set; }//datetime] NULL,
        public DateTime? DataDigitadoExame { get; set; }//datetime] NULL,        
        public DateTime? DataPendenteExame { get; set; }//datetime] NULL,
        public DateTime? DataImpressoExame { get; set; }//datetime] NULL,
        public DateTime? DataImporta { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataUsuarioCienteExame { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataImpSolicita { get; set; }//datetime] NULL,
        public DateTime? DataAlteradoExame { get; set; }//dbo].[TDateTime] NULL,
        public DateTime? DataEnvioEmail { get; set; }
        public string Observacao { get; set; }//varchar](1000) NULL,
        public string MotivoPendenteExame { get; set; }//varchar](200) NULL,
        public string ImpResultado { get; set; }//text] NULL,        
        public int? Quantidade { get; set; }//int] NULL,        
        public string MaqImpSolicita { get; set; }//varchar](100) NULL,
        public string VolumeMaterial { get; set; }//varchar](4) NULL,
        public string MaterialDescricaoLocal { get; set; }
        public string Mneumonico { get; set; }

        public long? ExameStatusId { get; set; }

        [ForeignKey("ExameStatusId")]
        public ExameStatus ExameStatus { get; set; }

        [ForeignKey("AutorizacaoProcedimentoItem"), Column("AteAutorizacaoProcedimentoitemId")]
        public long? AutorizacaoProcedimentoItemId { get; set; }
        public AutorizacaoProcedimentoItem AutorizacaoProcedimentoItem { get; set; }
        
        [ForeignKey("SolicitacaoExameItem"), Column("SolicitacaoExameItemId")]
        public long? SolicitacaoExameId { get; set; }
        
        public SolicitacaoExameItem SolicitacaoExameItem { get; set; }
        public bool IsPendencia { get; set; }
        public long? PendenciaUserId { get; set; }
        public DateTime? PendenciaDateTime { get; set; }
        
        public string MotivoPendencia { get; set; }
        
    }
}