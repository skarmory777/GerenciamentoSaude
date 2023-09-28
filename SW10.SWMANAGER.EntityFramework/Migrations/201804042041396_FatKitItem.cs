namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class FatKitItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatKitItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 255),
                    FatKitId = c.Long(),
                    FatItemId = c.Long(),
                    Quantidade = c.Int(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoKitItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.FatItemId)
                .ForeignKey("dbo.FatKit", t => t.FatKitId)
                .Index(t => t.FatKitId)
                .Index(t => t.FatItemId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatKitItem", "FatKitId", "dbo.FatKit");
            DropForeignKey("dbo.FatKitItem", "FatItemId", "dbo.FatItem");
            DropIndex("dbo.FatKitItem", new[] { "FatItemId" });
            DropIndex("dbo.FatKitItem", new[] { "FatKitId" });
            DropTable("dbo.FatKitItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoKitItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
