namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeEstoqueImportacaoProduto : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EstImportacaoProduto",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FornecedorId = c.Long(nullable: false),
                    ProdutoId = c.Long(nullable: false),
                    CNPJNota = c.String(),
                    CodigoProdutoNota = c.String(),
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
                    { "DynamicFilter_EstoqueImportacaoProduto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fornecedor", t => t.FornecedorId, cascadeDelete: true)
                .ForeignKey("dbo.Est_Produto", t => t.ProdutoId, cascadeDelete: true)
                .Index(t => t.FornecedorId)
                .Index(t => t.ProdutoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.EstImportacaoProduto", "ProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.EstImportacaoProduto", "FornecedorId", "dbo.Fornecedor");
            DropIndex("dbo.EstImportacaoProduto", new[] { "ProdutoId" });
            DropIndex("dbo.EstImportacaoProduto", new[] { "FornecedorId" });
            DropTable("dbo.EstImportacaoProduto",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_EstoqueImportacaoProduto_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
