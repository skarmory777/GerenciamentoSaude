namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeTransferenciaItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstoqueTransferenciaProdutoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PreMovimentoEntradaItemId = c.Long(nullable: false),
                    PreMovimentoSaidaItemId = c.Long(nullable: false),
                    EstoqueTransferenciaProdutoID = c.Long(nullable: false),
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
                    { "DynamicFilter_EstoqueTransferenciaProdutoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoqueTransferenciaProduto", t => t.EstoqueTransferenciaProdutoID, cascadeDelete: false)
                .ForeignKey("dbo.EstoquePreMovimentoItem", t => t.PreMovimentoEntradaItemId, cascadeDelete: false)
                .ForeignKey("dbo.EstoquePreMovimentoItem", t => t.PreMovimentoSaidaItemId, cascadeDelete: false)
                .Index(t => t.PreMovimentoEntradaItemId)
                .Index(t => t.PreMovimentoSaidaItemId)
                .Index(t => t.EstoqueTransferenciaProdutoID);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoqueTransferenciaProdutoItem", "PreMovimentoSaidaItemId", "dbo.EstoquePreMovimentoItem");
            DropForeignKey("dbo.EstoqueTransferenciaProdutoItem", "PreMovimentoEntradaItemId", "dbo.EstoquePreMovimentoItem");
            DropForeignKey("dbo.EstoqueTransferenciaProdutoItem", "EstoqueTransferenciaProdutoID", "dbo.EstoqueTransferenciaProduto");
            DropIndex("dbo.EstoqueTransferenciaProdutoItem", new[] { "EstoqueTransferenciaProdutoID" });
            DropIndex("dbo.EstoqueTransferenciaProdutoItem", new[] { "PreMovimentoSaidaItemId" });
            DropIndex("dbo.EstoqueTransferenciaProdutoItem", new[] { "PreMovimentoEntradaItemId" });
            DropTable("dbo.EstoqueTransferenciaProdutoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueTransferenciaProdutoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
