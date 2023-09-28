namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeExclusao_EstoqueTipoMovimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento");
            DropForeignKey("dbo.EstoqueMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento");
            DropIndex("dbo.EstoqueMovimento", new[] { "TipoMovimentoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "TipoMovimentoId" });
            DropTable("dbo.EstoqueTipoMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueTipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.EstoqueTipoMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_EstoqueTipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.EstoquePreMovimento", "TipoMovimentoId");
            CreateIndex("dbo.EstoqueMovimento", "TipoMovimentoId");
            AddForeignKey("dbo.EstoqueMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoquePreMovimento", "TipoMovimentoId", "dbo.EstoqueTipoMovimento", "Id", cascadeDelete: true);
        }
    }
}
