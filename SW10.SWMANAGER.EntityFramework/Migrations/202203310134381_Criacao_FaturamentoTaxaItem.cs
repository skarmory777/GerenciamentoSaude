namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations;
    
    public partial class Criacao_FaturamentoTaxaItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatTaxaItem",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FatTaxaId = c.Long(),
                        ItemId = c.Long(),
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
                    { "DynamicFilter_FaturamentoTaxaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatTaxa", t => t.FatTaxaId)
                .ForeignKey("dbo.FatItem", t => t.ItemId)
                .Index(t => t.FatTaxaId)
                .Index(t => t.ItemId);
            
            AddColumn("dbo.FatTaxa", "ItemId", c => c.Long());
            CreateIndex("dbo.FatTaxa", "ItemId");
            AddForeignKey("dbo.FatTaxa", "ItemId", "dbo.FatItem", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FatTaxaItem", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatTaxaItem", "FatTaxaId", "dbo.FatTaxa");
            DropForeignKey("dbo.FatTaxa", "ItemId", "dbo.FatItem");
            DropIndex("dbo.FatTaxaItem", new[] { "ItemId" });
            DropIndex("dbo.FatTaxaItem", new[] { "FatTaxaId" });
            DropIndex("dbo.FatTaxa", new[] { "ItemId" });
            DropColumn("dbo.FatTaxa", "ItemId");
            DropTable("dbo.FatTaxaItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTaxaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
