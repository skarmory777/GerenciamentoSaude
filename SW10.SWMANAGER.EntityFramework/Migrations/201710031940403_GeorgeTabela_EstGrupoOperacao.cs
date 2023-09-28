namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeTabela_EstGrupoOperacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstGrupoOperacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
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
                    { "DynamicFilter_EstoqueGrupoOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.EstoqueMovimento", "EstGrupoOperacaoId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "EstGrupoOperacaoId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "EstGrupoOperacaoId");
            CreateIndex("dbo.EstoquePreMovimento", "EstGrupoOperacaoId");
            AddForeignKey("dbo.EstoqueMovimento", "EstGrupoOperacaoId", "dbo.EstGrupoOperacao", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EstGrupoOperacaoId", "dbo.EstGrupoOperacao", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstGrupoOperacaoId", "dbo.EstGrupoOperacao");
            DropForeignKey("dbo.EstoqueMovimento", "EstGrupoOperacaoId", "dbo.EstGrupoOperacao");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstGrupoOperacaoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstGrupoOperacaoId" });
            DropColumn("dbo.EstoquePreMovimento", "EstGrupoOperacaoId");
            DropColumn("dbo.EstoqueMovimento", "EstGrupoOperacaoId");
            DropTable("dbo.EstGrupoOperacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueGrupoOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
