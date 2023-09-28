namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class FatItemConfig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatItemConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoItemConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId)
                .ForeignKey("dbo.FatItem", t => t.ItemId)
                .ForeignKey("dbo.FatItem", t => t.ItemCobrarId)
                .ForeignKey("dbo.SisPlano", t => t.PlanoId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId)
                .Index(t => t.ItemId)
                .Index(t => t.ItemCobrarId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItemConfig", "PlanoId", "dbo.SisPlano");
            DropForeignKey("dbo.FatItemConfig", "ItemCobrarId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemConfig", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemConfig", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatItemConfig", new[] { "ItemCobrarId" });
            DropIndex("dbo.FatItemConfig", new[] { "ItemId" });
            DropIndex("dbo.FatItemConfig", new[] { "PlanoId" });
            DropIndex("dbo.FatItemConfig", new[] { "ConvenioId" });
            DropTable("dbo.FatItemConfig",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoItemConfig_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
