namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class StatusSolicitacaoExameItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssStatusSolicitacaoExameItem",
                c => new
                {
                    Id = c.Long(nullable: false),
                    CorStatus = c.String(),
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
                    { "DynamicFilter_StatusSolicitacaoExameItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.AssSolicitacaoExameItem", "StatusSolicitacaoExameItemId", c => c.Long());
            CreateIndex("dbo.AssSolicitacaoExameItem", "StatusSolicitacaoExameItemId");
            AddForeignKey("dbo.AssSolicitacaoExameItem", "StatusSolicitacaoExameItemId", "dbo.AssStatusSolicitacaoExameItem", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoExameItem", "StatusSolicitacaoExameItemId", "dbo.AssStatusSolicitacaoExameItem");
            DropIndex("dbo.AssSolicitacaoExameItem", new[] { "StatusSolicitacaoExameItemId" });
            DropColumn("dbo.AssSolicitacaoExameItem", "StatusSolicitacaoExameItemId");
            DropTable("dbo.AssStatusSolicitacaoExameItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_StatusSolicitacaoExameItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
