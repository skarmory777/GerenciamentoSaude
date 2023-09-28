namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeSolicitacaoItens : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstSolicitacaoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ProdutoId = c.Long(nullable: false),
                    SolicitacaoId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    EstadoSolicitacaoItemId = c.Long(nullable: false),
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
                    { "DynamicFilter_EstoqueSolicitacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoquePreMovimentoEstado", t => t.EstadoSolicitacaoItemId, cascadeDelete: true)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: true)
                .ForeignKey("dbo.EstoquePreMovimentoItem", t => t.SolicitacaoId, cascadeDelete: true)
                .Index(t => t.ProdutoId)
                .Index(t => t.SolicitacaoId)
                .Index(t => t.EstadoSolicitacaoItemId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstSolicitacaoItem", "SolicitacaoId", "dbo.EstoquePreMovimentoItem");
            DropForeignKey("dbo.EstSolicitacaoItem", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstSolicitacaoItem", "EstadoSolicitacaoItemId", "dbo.EstoquePreMovimentoEstado");
            DropIndex("dbo.EstSolicitacaoItem", new[] { "EstadoSolicitacaoItemId" });
            DropIndex("dbo.EstSolicitacaoItem", new[] { "SolicitacaoId" });
            DropIndex("dbo.EstSolicitacaoItem", new[] { "ProdutoId" });
            DropTable("dbo.EstSolicitacaoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueSolicitacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
