namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_KitExameItem_campos_KitExame : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KitExameItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    KitExameId = c.Long(nullable: false),
                    FaturamentoItemId = c.Long(nullable: false),
                    IsLiberaKit = c.Boolean(nullable: false),
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
                    { "DynamicFilter_KitExameItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: false)
                .ForeignKey("dbo.LabKitExame", t => t.KitExameId, cascadeDelete: false)
                .Index(t => t.KitExameId)
                .Index(t => t.FaturamentoItemId);

            AddColumn("dbo.LabKitExame", "IsAtivo", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropForeignKey("dbo.KitExameItem", "KitExameId", "dbo.LabKitExame");
            DropForeignKey("dbo.KitExameItem", "FaturamentoItemId", "dbo.FatItem");
            DropIndex("dbo.KitExameItem", new[] { "FaturamentoItemId" });
            DropIndex("dbo.KitExameItem", new[] { "KitExameId" });
            DropColumn("dbo.LabKitExame", "IsAtivo");
            DropTable("dbo.KitExameItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_KitExameItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
