namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class George_EstMovimentoBaixaItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstMovimentoBaixaItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstoqueMovimentoBaixaId = c.Long(nullable: false),
                    EstoqueMovimentoItemId = c.Long(nullable: false),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_EstMovimentoBaixaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EstoqueMovimento", t => t.EstoqueMovimentoBaixaId, cascadeDelete: true)
                .ForeignKey("dbo.EstoqueMovimentoItem", t => t.EstoqueMovimentoItemId, cascadeDelete: true)
                .Index(t => t.EstoqueMovimentoBaixaId)
                .Index(t => t.EstoqueMovimentoItemId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstMovimentoBaixaItem", "EstoqueMovimentoItemId", "dbo.EstoqueMovimentoItem");
            DropForeignKey("dbo.EstMovimentoBaixaItem", "EstoqueMovimentoBaixaId", "dbo.EstoqueMovimento");
            DropIndex("dbo.EstMovimentoBaixaItem", new[] { "EstoqueMovimentoItemId" });
            DropIndex("dbo.EstMovimentoBaixaItem", new[] { "EstoqueMovimentoBaixaId" });
            DropTable("dbo.EstMovimentoBaixaItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstMovimentoBaixaItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
