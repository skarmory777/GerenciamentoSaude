namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class George_Inclusao_EstTipoMovimento_EstTipoOperacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstTipoMovimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsEntrada = c.Boolean(nullable: false),
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
                    { "DynamicFilter_TipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.EstTipoOperacao",
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
                    { "DynamicFilter_TipoOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.EstoqueMovimento", "EstTipoMovimentoId", c => c.Long());
            AddColumn("dbo.EstoqueMovimento", "EstTipoOperacaoId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "EstTipoMovimentoId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "EstTipoOperacaoId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "EstTipoMovimentoId");
            CreateIndex("dbo.EstoqueMovimento", "EstTipoOperacaoId");
            CreateIndex("dbo.EstoquePreMovimento", "EstTipoMovimentoId");
            CreateIndex("dbo.EstoquePreMovimento", "EstTipoOperacaoId");
            AddForeignKey("dbo.EstoquePreMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao", "Id");
            DropColumn("dbo.EstoqueMovimento", "TipoMovimentoId");
            DropColumn("dbo.EstoqueMovimento", "TipoDocumentoId");
            DropColumn("dbo.EstoquePreMovimento", "TipoMovimentoId");
            DropColumn("dbo.EstoquePreMovimento", "TipoDocumentoId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstoquePreMovimento", "TipoDocumentoId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "TipoMovimentoId", c => c.Long(nullable: false));
            AddColumn("dbo.EstoqueMovimento", "TipoDocumentoId", c => c.Long());
            AddColumn("dbo.EstoqueMovimento", "TipoMovimentoId", c => c.Long(nullable: false));
            DropForeignKey("dbo.EstoqueMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao");
            DropForeignKey("dbo.EstoqueMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento");
            DropForeignKey("dbo.EstoquePreMovimento", "EstTipoOperacaoId", "dbo.EstTipoOperacao");
            DropForeignKey("dbo.EstoquePreMovimento", "EstTipoMovimentoId", "dbo.EstTipoMovimento");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstTipoOperacaoId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstTipoMovimentoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstTipoOperacaoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstTipoMovimentoId" });
            DropColumn("dbo.EstoquePreMovimento", "EstTipoOperacaoId");
            DropColumn("dbo.EstoquePreMovimento", "EstTipoMovimentoId");
            DropColumn("dbo.EstoqueMovimento", "EstTipoOperacaoId");
            DropColumn("dbo.EstoqueMovimento", "EstTipoMovimentoId");
            DropTable("dbo.EstTipoOperacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.EstTipoMovimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoMovimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
