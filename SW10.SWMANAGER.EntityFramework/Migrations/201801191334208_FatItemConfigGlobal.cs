namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class FatItemConfigGlobal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatGlobal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
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
                    { "DynamicFilter_FaturamentoGlobal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FatItemConfigGlobal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    GlobalId = c.Long(),
                    ItemId = c.Long(),
                    ItemCobrarId = c.Long(),
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
                    { "DynamicFilter_FaturamentoItemConfigGlobal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGlobal", t => t.GlobalId)
                .ForeignKey("dbo.FatItem", t => t.ItemId)
                .ForeignKey("dbo.FatItem", t => t.ItemCobrarId)
                .Index(t => t.GlobalId)
                .Index(t => t.ItemId)
                .Index(t => t.ItemCobrarId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItemConfigGlobal", "ItemCobrarId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemConfigGlobal", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemConfigGlobal", "GlobalId", "dbo.FatGlobal");
            DropIndex("dbo.FatItemConfigGlobal", new[] { "ItemCobrarId" });
            DropIndex("dbo.FatItemConfigGlobal", new[] { "ItemId" });
            DropIndex("dbo.FatItemConfigGlobal", new[] { "GlobalId" });
            DropTable("dbo.FatItemConfigGlobal",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoItemConfigGlobal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatGlobal",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGlobal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
