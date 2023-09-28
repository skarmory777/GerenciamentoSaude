namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class RateioCentroCusto : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinGrupoContaAdministrativa", "GrupoDREId", "dbo.FinGrupoDRE");
            DropIndex("dbo.FinGrupoContaAdministrativa", new[] { "GrupoDREId" });
            CreateTable(
                "dbo.FinRateioCentroCustoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    RateioCentroCustoId = c.Long(),
                    PercentualRateio = c.Decimal(nullable: false, precision: 18, scale: 2),
                    CentroCustoId = c.Long(nullable: false),
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
                    { "DynamicFilter_RateioCentroCustoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CentroCusto", t => t.CentroCustoId, cascadeDelete: true)
                .ForeignKey("dbo.FinRateioCentroCusto", t => t.RateioCentroCustoId)
                .Index(t => t.RateioCentroCustoId)
                .Index(t => t.CentroCustoId);

            CreateTable(
                "dbo.FinRateioCentroCusto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_RateioCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AlterColumn("dbo.FinGrupoContaAdministrativa", "GrupoDREId", c => c.Long());
            CreateIndex("dbo.FinGrupoContaAdministrativa", "GrupoDREId");
            AddForeignKey("dbo.FinGrupoContaAdministrativa", "GrupoDREId", "dbo.FinGrupoDRE", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinGrupoContaAdministrativa", "GrupoDREId", "dbo.FinGrupoDRE");
            DropForeignKey("dbo.FinRateioCentroCustoItem", "RateioCentroCustoId", "dbo.FinRateioCentroCusto");
            DropForeignKey("dbo.FinRateioCentroCustoItem", "CentroCustoId", "dbo.CentroCusto");
            DropIndex("dbo.FinRateioCentroCustoItem", new[] { "CentroCustoId" });
            DropIndex("dbo.FinRateioCentroCustoItem", new[] { "RateioCentroCustoId" });
            DropIndex("dbo.FinGrupoContaAdministrativa", new[] { "GrupoDREId" });
            AlterColumn("dbo.FinGrupoContaAdministrativa", "GrupoDREId", c => c.Long(nullable: false));
            DropTable("dbo.FinRateioCentroCusto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RateioCentroCusto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinRateioCentroCustoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_RateioCentroCustoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            CreateIndex("dbo.FinGrupoContaAdministrativa", "GrupoDREId");
            AddForeignKey("dbo.FinGrupoContaAdministrativa", "GrupoDREId", "dbo.FinGrupoDRE", "Id", cascadeDelete: true);
        }
    }
}
