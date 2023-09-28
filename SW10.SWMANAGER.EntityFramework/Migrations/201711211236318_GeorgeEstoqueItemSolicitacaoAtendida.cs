namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeEstoqueItemSolicitacaoAtendida : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstItemSolicitacaoAtendida",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SolicitacaoItemId = c.Long(nullable: false),
                    PreMovimentoItemId = c.Long(nullable: false),
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
                    { "DynamicFilter_EstoqueItemSolicitacaoAtendida_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoquePreMovimentoItem", t => t.PreMovimentoItemId, cascadeDelete: true)
                .ForeignKey("dbo.EstSolicitacaoItem", t => t.SolicitacaoItemId, cascadeDelete: true)
                .Index(t => t.SolicitacaoItemId)
                .Index(t => t.PreMovimentoItemId);

            AddColumn("dbo.EstSolicitacaoItem", "QuantidadeAtendida", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstItemSolicitacaoAtendida", "SolicitacaoItemId", "dbo.EstSolicitacaoItem");
            DropForeignKey("dbo.EstItemSolicitacaoAtendida", "PreMovimentoItemId", "dbo.EstoquePreMovimentoItem");
            DropIndex("dbo.EstItemSolicitacaoAtendida", new[] { "PreMovimentoItemId" });
            DropIndex("dbo.EstItemSolicitacaoAtendida", new[] { "SolicitacaoItemId" });
            DropColumn("dbo.EstSolicitacaoItem", "QuantidadeAtendida");
            DropTable("dbo.EstItemSolicitacaoAtendida",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueItemSolicitacaoAtendida_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
