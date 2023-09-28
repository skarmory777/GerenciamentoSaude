namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoEstoquePreMovimento4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId");
            AddForeignKey("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", "dbo.Est_Unidade", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.EstoquePreMovimentoItem", new[] { "ProdutoUnidadeId" });
            DropColumn("dbo.EstoquePreMovimentoItem", "ProdutoUnidadeId");
        }
    }
}
