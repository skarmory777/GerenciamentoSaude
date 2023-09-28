namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class InclusaoFormulaEstoqueKit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssFormulaEstoqueKit",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AssPrescricaoItemId = c.Long(nullable: false),
                    AssEstoqueKitId = c.Long(nullable: false),
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
                    { "DynamicFilter_FormulaEstoqueKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstKit", t => t.AssEstoqueKitId, cascadeDelete: true)
                .ForeignKey("dbo.AssPrescricaoItem", t => t.AssPrescricaoItemId, cascadeDelete: true)
                .Index(t => t.AssPrescricaoItemId)
                .Index(t => t.AssEstoqueKitId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AssFormulaEstoqueKit", "AssPrescricaoItemId", "dbo.AssPrescricaoItem");
            DropForeignKey("dbo.AssFormulaEstoqueKit", "AssEstoqueKitId", "dbo.EstKit");
            DropIndex("dbo.AssFormulaEstoqueKit", new[] { "AssEstoqueKitId" });
            DropIndex("dbo.AssFormulaEstoqueKit", new[] { "AssPrescricaoItemId" });
            DropTable("dbo.AssFormulaEstoqueKit",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaEstoqueKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
