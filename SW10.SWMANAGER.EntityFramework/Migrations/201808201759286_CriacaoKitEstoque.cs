namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class CriacaoKitEstoque : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstKitItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoqueKitId = c.Long(),
                    Quantidade = c.Int(nullable: false),
                    ProdutoId = c.Long(nullable: false),
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
                    { "DynamicFilter_EstoqueKitItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstKit", t => t.EstoqueKitId)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: true)
                .Index(t => t.EstoqueKitId)
                .Index(t => t.ProdutoId);

            CreateTable(
                "dbo.EstKit",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_EstoqueKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstKitItem", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstKitItem", "EstoqueKitId", "dbo.EstKit");
            DropIndex("dbo.EstKitItem", new[] { "ProdutoId" });
            DropIndex("dbo.EstKitItem", new[] { "EstoqueKitId" });
            DropTable("dbo.EstKit",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueKit_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstKitItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueKitItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
