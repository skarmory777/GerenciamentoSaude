namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class pedidos_do_leao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssDivisao", "IsEstoque", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssDivisao", "IsFaturamento", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItem", "EstProdutoId", c => c.Long());
            AddColumn("dbo.AssPrescricaoItem", "FatItemId", c => c.Long());
            CreateIndex("dbo.AssPrescricaoItem", "EstProdutoId");
            CreateIndex("dbo.AssPrescricaoItem", "FatItemId");
            AddForeignKey("dbo.AssPrescricaoItem", "FatItemId", "dbo.FatItem", "Id");
            AddForeignKey("dbo.AssPrescricaoItem", "EstProdutoId", "dbo.Est_Produto", "Id");
            DropColumn("dbo.AssFormulaEstoque", "IsPrincipal");
            DropColumn("dbo.AssFormulaFaturamento", "IsFatura");
        }

        public override void Down()
        {
            AddColumn("dbo.AssFormulaFaturamento", "IsFatura", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssFormulaEstoque", "IsPrincipal", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AssPrescricaoItem", "EstProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.AssPrescricaoItem", "FatItemId", "dbo.FatItem");
            DropIndex("dbo.AssPrescricaoItem", new[] { "FatItemId" });
            DropIndex("dbo.AssPrescricaoItem", new[] { "EstProdutoId" });
            DropColumn("dbo.AssPrescricaoItem", "FatItemId");
            DropColumn("dbo.AssPrescricaoItem", "EstProdutoId");
            DropColumn("dbo.AssDivisao", "IsFaturamento");
            DropColumn("dbo.AssDivisao", "IsEstoque");
        }
    }
}
