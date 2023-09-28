namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class ContaAdministrativa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinContaAdministrativa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsNaoContabilizarPagarGerencial = c.Boolean(nullable: false),
                    IsNaoContabilizarReceberGerencial = c.Boolean(nullable: false),
                    IsReceita = c.Boolean(nullable: false),
                    IsDespesa = c.Boolean(nullable: false),
                    RateioCentroCustoId = c.Long(),
                    SubGrupoContaAdministrativaId = c.Long(nullable: false),
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
                    { "DynamicFilter_ContaAdministrativa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinRateioCentroCusto", t => t.RateioCentroCustoId)
                .ForeignKey("dbo.FinSubGrupoContaAdministrativa", t => t.SubGrupoContaAdministrativaId, cascadeDelete: true)
                .Index(t => t.RateioCentroCustoId)
                .Index(t => t.SubGrupoContaAdministrativaId);

            CreateTable(
                "dbo.FinContaAdministrativaCentroCusto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ContaAdministrativaId = c.Long(),
                    CentroCustoId = c.Long(nullable: false),
                    Percentual = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_ContaAdministrativaCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId, cascadeDelete: true)
                .ForeignKey("dbo.FinContaAdministrativa", t => t.ContaAdministrativaId)
                .Index(t => t.ContaAdministrativaId)
                .Index(t => t.CentroCustoId);

            CreateTable(
                "dbo.FinContaAdministrativaEmpresa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ContaAdministrativaId = c.Long(),
                    EnpresaId = c.Long(nullable: false),
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
                    { "DynamicFilter_ContaAdministrativaEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinContaAdministrativa", t => t.ContaAdministrativaId)
                .ForeignKey("dbo.SisEmpresa", t => t.EnpresaId, cascadeDelete: true)
                .Index(t => t.ContaAdministrativaId)
                .Index(t => t.EnpresaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FinContaAdministrativaEmpresa", "EnpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FinContaAdministrativaEmpresa", "ContaAdministrativaId", "dbo.FinContaAdministrativa");
            DropForeignKey("dbo.FinContaAdministrativaCentroCusto", "ContaAdministrativaId", "dbo.FinContaAdministrativa");
            DropForeignKey("dbo.FinContaAdministrativaCentroCusto", "CentroCustoId", "dbo.CentroCusto");
            DropForeignKey("dbo.FinContaAdministrativa", "SubGrupoContaAdministrativaId", "dbo.FinSubGrupoContaAdministrativa");
            DropForeignKey("dbo.FinContaAdministrativa", "RateioCentroCustoId", "dbo.FinRateioCentroCusto");
            DropIndex("dbo.FinContaAdministrativaEmpresa", new[] { "EnpresaId" });
            DropIndex("dbo.FinContaAdministrativaEmpresa", new[] { "ContaAdministrativaId" });
            DropIndex("dbo.FinContaAdministrativaCentroCusto", new[] { "CentroCustoId" });
            DropIndex("dbo.FinContaAdministrativaCentroCusto", new[] { "ContaAdministrativaId" });
            DropIndex("dbo.FinContaAdministrativa", new[] { "SubGrupoContaAdministrativaId" });
            DropIndex("dbo.FinContaAdministrativa", new[] { "RateioCentroCustoId" });
            DropTable("dbo.FinContaAdministrativaEmpresa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContaAdministrativaEmpresa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinContaAdministrativaCentroCusto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContaAdministrativaCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinContaAdministrativa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ContaAdministrativa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
