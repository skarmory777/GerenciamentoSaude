namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_Tabelas_ReqExame_VisualAsa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pro_ReqExameMovItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdRequisicaomovItem = c.Int(nullable: false),
                    QtdeRequisitada = c.Int(nullable: false),
                    DataAtualizacao = c.DateTime(nullable: false),
                    IsEncerrada = c.Boolean(nullable: false),
                    IsAtendida = c.Boolean(nullable: false),
                    IdRequisicaoMov = c.Int(nullable: false),
                    IdUsuario = c.Int(nullable: false),
                    IdItemRequisitado = c.Int(nullable: false),
                    IdItem = c.Int(nullable: false),
                    IdAutorizacao = c.Int(nullable: false),
                    DataAutorizacao = c.DateTime(nullable: false),
                    SenhaAutorizacao = c.String(maxLength: 20),
                    NomeAutorizacao = c.String(maxLength: 50),
                    ObsAutorizacao = c.String(maxLength: 100),
                    IdFatKit = c.Int(),
                    IdMaterial = c.Int(),
                    ObsRequisicao = c.String(maxLength: 1000),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_Pro_ReqExameMovItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Pro_ReqExameMov",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IdRequisicaoMov = c.Int(nullable: false),
                    IdCcRequisitado = c.Int(nullable: false),
                    IdMedico = c.Int(nullable: false),
                    DataRequisicao = c.DateTime(nullable: false),
                    NumeroDocumento = c.String(maxLength: 20),
                    IsEncerrada = c.Boolean(nullable: false),
                    IsSemanal = c.Boolean(nullable: false),
                    Obs = c.String(maxLength: 255),
                    DataAutorizacao = c.DateTime(nullable: false),
                    IdUsuario = c.Int(nullable: false),
                    IdAtendimento = c.Int(nullable: false),
                    IdLocalUtilizacao = c.Int(nullable: false),
                    IdTerceirizado = c.Int(nullable: false),
                    IdCentroCusto = c.Int(nullable: false),
                    Hidden = c.Boolean(nullable: false),
                    IdReqExameStatus = c.Int(nullable: false),
                    IdClinica = c.Int(),
                    TipoRequisicao = c.Int(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_Pro_ReqExameMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropTable("dbo.Pro_ReqExameMov",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Pro_ReqExameMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.Pro_ReqExameMovItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Pro_ReqExameMovItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
