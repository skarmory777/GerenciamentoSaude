namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class add_compra_cotacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmpCotacaoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    CmpRequisicaoItemId = c.Long(nullable: false),
                    FornecedorId = c.Long(nullable: false),
                    Preco = c.Decimal(nullable: false, precision: 18, scale: 2),
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
                    { "DynamicFilter_CompraCotacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: true)
                .ForeignKey("dbo.CmpRequisicaoItem", t => t.CmpRequisicaoItemId, cascadeDelete: true)
                .Index(t => t.CmpRequisicaoItemId)
                .Index(t => t.FornecedorId);

            CreateTable(
                "dbo.CmpCotacaoStatus",
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
                    { "DynamicFilter_CompraCotacaoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            AddColumn("dbo.CmpRequisicao", "CmpCotacaoStatusId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpRequisicao", "CmpCotacaoStatusId");
            AddForeignKey("dbo.CmpRequisicao", "CmpCotacaoStatusId", "dbo.CmpCotacaoStatus", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpCotacaoItem", "CmpRequisicaoItemId", "dbo.CmpRequisicaoItem");
            DropForeignKey("dbo.CmpRequisicao", "CmpCotacaoStatusId", "dbo.CmpCotacaoStatus");
            DropForeignKey("dbo.CmpCotacaoItem", "FornecedorId", "dbo.Fornecedor");
            DropIndex("dbo.CmpRequisicao", new[] { "CmpCotacaoStatusId" });
            DropIndex("dbo.CmpCotacaoItem", new[] { "FornecedorId" });
            DropIndex("dbo.CmpCotacaoItem", new[] { "CmpRequisicaoItemId" });
            DropColumn("dbo.CmpRequisicao", "CmpCotacaoStatusId");
            DropTable("dbo.CmpCotacaoStatus",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraCotacaoStatus_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CmpCotacaoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraCotacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
