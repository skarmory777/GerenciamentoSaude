namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class add_aprovacao_status : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmpAprovacaoStatus",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_CompraAprovacaoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.CmpRequisicao", "CmpAprovacaoStatusId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicao", "CmpAprovacaoStatusId");
            AddForeignKey("dbo.CmpRequisicao", "CmpAprovacaoStatusId", "dbo.CmpAprovacaoStatus", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicao", "CmpAprovacaoStatusId", "dbo.CmpAprovacaoStatus");
            DropIndex("dbo.CmpRequisicao", new[] { "CmpAprovacaoStatusId" });
            DropColumn("dbo.CmpRequisicao", "CmpAprovacaoStatusId");
            DropTable("dbo.CmpAprovacaoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraAprovacaoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
