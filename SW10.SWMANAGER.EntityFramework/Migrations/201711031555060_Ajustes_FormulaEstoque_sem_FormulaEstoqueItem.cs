namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Ajustes_FormulaEstoque_sem_FormulaEstoqueItem : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssFormulaEstoqueItem", "AssFormulaEstoqueId", "dbo.AssFormulaEstoque");
            DropForeignKey("dbo.AssFormulaEstoqueItem", "EstProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.AssFormulaEstoqueItem", "EstUnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.AssFormulaEstoqueItem", new[] { "EstProdutoId" });
            DropIndex("dbo.AssFormulaEstoqueItem", new[] { "EstUnidadeId" });
            DropIndex("dbo.AssFormulaEstoqueItem", new[] { "AssFormulaEstoqueId" });
            RenameColumn(table: "dbo.AssFormulaFaturamento", name: "FatItem", newName: "FatItemId");
            RenameIndex(table: "dbo.AssFormulaFaturamento", name: "IX_FatItem", newName: "IX_FatItemId");
            AddColumn("dbo.AssFormulaEstoque", "IsPrincipal", c => c.Boolean(nullable: false));
            DropTable("dbo.AssFormulaEstoqueItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaEstoqueItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.AssFormulaEstoqueItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstProdutoId = c.Long(),
                    EstUnidadeId = c.Long(),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsVisivel = c.Boolean(nullable: false),
                    IsGeraSolicitacaoEstoque = c.Boolean(nullable: false),
                    Descricao = c.String(),
                    AssFormulaEstoqueId = c.Long(nullable: false),
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
                    { "DynamicFilter_FormulaEstoqueItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            DropColumn("dbo.AssFormulaEstoque", "IsPrincipal");
            RenameIndex(table: "dbo.AssFormulaFaturamento", name: "IX_FatItemId", newName: "IX_FatItem");
            RenameColumn(table: "dbo.AssFormulaFaturamento", name: "FatItemId", newName: "FatItem");
            CreateIndex("dbo.AssFormulaEstoqueItem", "AssFormulaEstoqueId");
            CreateIndex("dbo.AssFormulaEstoqueItem", "EstUnidadeId");
            CreateIndex("dbo.AssFormulaEstoqueItem", "EstProdutoId");
            AddForeignKey("dbo.AssFormulaEstoqueItem", "EstUnidadeId", "dbo.Est_Unidade", "Id");
            AddForeignKey("dbo.AssFormulaEstoqueItem", "EstProdutoId", "dbo.Est_Produto", "Id");
            AddForeignKey("dbo.AssFormulaEstoqueItem", "AssFormulaEstoqueId", "dbo.AssFormulaEstoque", "Id", cascadeDelete: true);
        }
    }
}
