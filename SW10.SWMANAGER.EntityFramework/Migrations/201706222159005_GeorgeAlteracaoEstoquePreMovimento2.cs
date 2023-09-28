namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoEstoquePreMovimento2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "ProdutoUnidadeId" });
            DropForeignKey("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", "dbo.Est_ProdutoUnidade");

            AddColumn("dbo.EstoquePreMovimentoItem", "CodigoBarra", c => c.String());
            DropColumn("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", c => c.Long());
            DropColumn("dbo.EstoquePreMovimentoItem", "CodigoBarra");
            CreateIndex("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId");
            AddForeignKey("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", "dbo.Est_ProdutoUnidade", "Id");
        }
    }
}
