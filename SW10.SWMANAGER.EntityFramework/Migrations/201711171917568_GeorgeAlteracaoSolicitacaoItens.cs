namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoSolicitacaoItens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstSolicitacaoItem", "ProdutoUnidadeId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstSolicitacaoItem", "ProdutoUnidadeId");
            AddForeignKey("dbo.EstSolicitacaoItem", "ProdutoUnidadeId", "dbo.Est_Unidade", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstSolicitacaoItem", "ProdutoUnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.EstSolicitacaoItem", new[] { "ProdutoUnidadeId" });
            DropColumn("dbo.EstSolicitacaoItem", "ProdutoUnidadeId");
        }
    }
}
