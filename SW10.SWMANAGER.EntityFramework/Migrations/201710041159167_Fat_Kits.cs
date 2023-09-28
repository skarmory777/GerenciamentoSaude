namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_Kits : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatKit",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Observacao = c.String(maxLength: 255),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_FaturamentoKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FaturamentoKitFaturamentoItems",
                c => new
                {
                    FaturamentoKit_Id = c.Long(nullable: false),
                    FaturamentoItem_Id = c.Long(nullable: false),
                })
                .PrimaryKey(t => new { t.FaturamentoKit_Id, t.FaturamentoItem_Id })
                .ForeignKey("dbo.FatKit", t => t.FaturamentoKit_Id, cascadeDelete: false)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItem_Id, cascadeDelete: false)
                .Index(t => t.FaturamentoKit_Id)
                .Index(t => t.FaturamentoItem_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FaturamentoKitFaturamentoItems", "FaturamentoItem_Id", "dbo.FatItem");
            DropForeignKey("dbo.FaturamentoKitFaturamentoItems", "FaturamentoKit_Id", "dbo.FatKit");
            DropIndex("dbo.FaturamentoKitFaturamentoItems", new[] { "FaturamentoItem_Id" });
            DropIndex("dbo.FaturamentoKitFaturamentoItems", new[] { "FaturamentoKit_Id" });
            DropTable("dbo.FaturamentoKitFaturamentoItems");
            DropTable("dbo.FatKit",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
