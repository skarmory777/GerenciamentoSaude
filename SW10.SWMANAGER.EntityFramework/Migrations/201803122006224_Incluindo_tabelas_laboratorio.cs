namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Incluindo_tabelas_laboratorio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabResultado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TecnicoId = c.Long(),
                    FaturamentoContaId = c.Long(),
                    ResponsavelId = c.Long(),
                    UsuarioConferidoId = c.Long(),
                    UsuarioDigitadoId = c.Long(),
                    UsuarioEntregaId = c.Long(),
                    UsuarioCienteId = c.Long(),
                    TecnicoColetaId = c.Long(),
                    LeitoAtualId = c.Long(),
                    LocalAtualId = c.Long(),
                    RotinaId = c.Long(),
                    RequisicaoMovId = c.Long(),
                    IsRn = c.Boolean(nullable: false),
                    IsEmail = c.Boolean(nullable: false),
                    IsEmergencia = c.Boolean(nullable: false),
                    IsUrgente = c.Boolean(nullable: false),
                    IsAvisoLab = c.Boolean(nullable: false),
                    IsAvisoMed = c.Boolean(nullable: false),
                    IsVisualiza = c.Boolean(nullable: false),
                    IsRotina = c.Boolean(nullable: false),
                    IsTransferencia = c.Boolean(nullable: false),
                    IsCiente = c.Boolean(nullable: false),
                    Numero = c.String(),
                    DataColeta = c.DateTime(),
                    SexoRnId = c.Long(),
                    DataDigitado = c.DateTime(),
                    DataConferido = c.DateTime(),
                    DataEnvioEmail = c.DateTime(),
                    DataEntregaExame = c.DateTime(),
                    ObsEntrega = c.String(),
                    PessoaEntrega = c.String(),
                    DataPrevEntregaExame = c.DateTime(),
                    Gemelar = c.String(),
                    DataTecnico = c.DateTime(),
                    DataUsuarioCiente = c.DateTime(),
                    Peso = c.String(),
                    Altura = c.String(),
                    Remedio = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Resultado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatConta", t => t.FaturamentoContaId)
                .ForeignKey("dbo.AteLeito", t => t.LeitoAtualId)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.LocalAtualId)
                .ForeignKey("dbo.LabTecnico", t => t.ResponsavelId)
                .ForeignKey("dbo.LabTecnico", t => t.TecnicoId)
                .ForeignKey("dbo.LabTecnico", t => t.TecnicoColetaId)
                .ForeignKey("dbo.AbpUsers", t => t.UsuarioCienteId)
                .ForeignKey("dbo.AbpUsers", t => t.UsuarioConferidoId)
                .ForeignKey("dbo.AbpUsers", t => t.UsuarioDigitadoId)
                .ForeignKey("dbo.AbpUsers", t => t.UsuarioEntregaId)
                .Index(t => t.TecnicoId)
                .Index(t => t.FaturamentoContaId)
                .Index(t => t.ResponsavelId)
                .Index(t => t.UsuarioConferidoId)
                .Index(t => t.UsuarioDigitadoId)
                .Index(t => t.UsuarioEntregaId)
                .Index(t => t.UsuarioCienteId)
                .Index(t => t.TecnicoColetaId)
                .Index(t => t.LeitoAtualId)
                .Index(t => t.LocalAtualId);

            CreateTable(
                "dbo.LabResultadoExame",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FormataId = c.Long(),
                    UsuarioExclusaoId = c.Long(),
                    UsuarioAlteracaoId = c.Long(),
                    UsuarioInclusaoId = c.Long(),
                    ExameId = c.Long(),
                    ResultadoId = c.Long(),
                    FatContaItemId = c.Long(),
                    KitId = c.Long(),
                    UsuarioConferidoExameId = c.Long(),
                    UsuarioDigitadoExameId = c.Long(),
                    UsuarioPendenteExameId = c.Long(),
                    UsuarioImpressoExameId = c.Long(),
                    MaterialExameId = c.Long(),
                    UsuarioCienteExameId = c.Long(),
                    UsuarioImpSolicitaId = c.Long(),
                    UsuarioAlteradoExameId = c.Long(),
                    TabelaId = c.Long(),
                    IsImprime = c.Boolean(nullable: false),
                    IsSigiloso = c.Boolean(nullable: false),
                    IsSergioFranco = c.Boolean(nullable: false),
                    IsCienteExame = c.Boolean(nullable: false),
                    Seq = c.Int(nullable: false),
                    DataInclusao = c.DateTime(nullable: false),
                    DataAlteracao = c.DateTime(nullable: false),
                    DataExclusao = c.DateTime(nullable: false),
                    ImpResultado = c.String(),
                    QtdExame = c.Int(),
                    DataConferidoExame = c.DateTime(nullable: false),
                    DataDigitadoExame = c.DateTime(nullable: false),
                    DataPendenteExame = c.DateTime(nullable: false),
                    MotivoPendenteExame = c.String(),
                    DataImpressoExame = c.DateTime(nullable: false),
                    DataImporta = c.DateTime(nullable: false),
                    ObsExame = c.String(),
                    DataUsuarioCienteExame = c.DateTime(nullable: false),
                    DataImpSolicita = c.DateTime(nullable: false),
                    MaqImpSolicita = c.String(),
                    VolumeMaterial = c.String(),
                    DataAlteradoExame = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResultadoExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.LabTabela", t => t.TabelaId)
                .Index(t => t.TabelaId);

            CreateTable(
                "dbo.LabResultadoLaudo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ResultadoExameId = c.Long(),
                    ItemResultadoId = c.Long(),
                    TabelaResultadoId = c.Long(),
                    UnidadeId = c.Long(),
                    UsuarioLaudoId = c.Long(),
                    Numerico = c.Double(nullable: false),
                    Resultado = c.String(),
                    Referencia = c.String(),
                    DataDigitadoLaudo = c.DateTime(),
                    VersaoAtual = c.String(),
                    IsInterface = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResultadoLaudo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "TabelaId", "dbo.LabTabela");
            DropForeignKey("dbo.LabResultado", "UsuarioEntregaId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultado", "UsuarioDigitadoId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultado", "UsuarioConferidoId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultado", "UsuarioCienteId", "dbo.AbpUsers");
            DropForeignKey("dbo.LabResultado", "TecnicoColetaId", "dbo.LabTecnico");
            DropForeignKey("dbo.LabResultado", "TecnicoId", "dbo.LabTecnico");
            DropForeignKey("dbo.LabResultado", "ResponsavelId", "dbo.LabTecnico");
            DropForeignKey("dbo.LabResultado", "LocalAtualId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.LabResultado", "LeitoAtualId", "dbo.AteLeito");
            DropForeignKey("dbo.LabResultado", "FaturamentoContaId", "dbo.FatConta");
            DropIndex("dbo.LabResultadoExame", new[] { "TabelaId" });
            DropIndex("dbo.LabResultado", new[] { "LocalAtualId" });
            DropIndex("dbo.LabResultado", new[] { "LeitoAtualId" });
            DropIndex("dbo.LabResultado", new[] { "TecnicoColetaId" });
            DropIndex("dbo.LabResultado", new[] { "UsuarioCienteId" });
            DropIndex("dbo.LabResultado", new[] { "UsuarioEntregaId" });
            DropIndex("dbo.LabResultado", new[] { "UsuarioDigitadoId" });
            DropIndex("dbo.LabResultado", new[] { "UsuarioConferidoId" });
            DropIndex("dbo.LabResultado", new[] { "ResponsavelId" });
            DropIndex("dbo.LabResultado", new[] { "FaturamentoContaId" });
            DropIndex("dbo.LabResultado", new[] { "TecnicoId" });
            DropTable("dbo.LabResultadoLaudo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResultadoLaudo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabResultadoExame",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ResultadoExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabResultado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Resultado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
