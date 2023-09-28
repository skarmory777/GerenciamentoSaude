namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeTabelaTransferenciaProduto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstoqueTransferenciaProduto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    PreMovimentoEntradaId = c.Long(nullable: false),
                    PreMovimentoSaidaId = c.Long(nullable: false),
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
                    { "DynamicFilter_EstoqueTransferenciaProduto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoquePreMovimento", t => t.PreMovimentoEntradaId, cascadeDelete: false)
                .ForeignKey("dbo.EstoquePreMovimento", t => t.PreMovimentoSaidaId, cascadeDelete: false)
                .Index(t => t.PreMovimentoEntradaId)
                .Index(t => t.PreMovimentoSaidaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoqueTransferenciaProduto", "PreMovimentoSaidaId", "dbo.EstoquePreMovimento");
            DropForeignKey("dbo.EstoqueTransferenciaProduto", "PreMovimentoEntradaId", "dbo.EstoquePreMovimento");
            DropIndex("dbo.EstoqueTransferenciaProduto", new[] { "PreMovimentoSaidaId" });
            DropIndex("dbo.EstoqueTransferenciaProduto", new[] { "PreMovimentoEntradaId" });
            DropTable("dbo.EstoqueTransferenciaProduto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueTransferenciaProduto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
