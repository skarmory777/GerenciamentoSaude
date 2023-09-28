namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoSolicitacaoItens1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstSolicitacaoItem", "SolicitacaoId", "dbo.EstoquePreMovimentoItem");
            DropIndex("dbo.EstSolicitacaoItem", new[] { "SolicitacaoId" });
            DropColumn("dbo.EstSolicitacaoItem", "SolicitacaoId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstSolicitacaoItem", "SolicitacaoId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstSolicitacaoItem", "SolicitacaoId");
            AddForeignKey("dbo.EstSolicitacaoItem", "SolicitacaoId", "dbo.EstoquePreMovimentoItem", "Id", cascadeDelete: true);
        }
    }
}
