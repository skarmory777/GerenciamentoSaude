namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoSolicitacaoItens2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstSolicitacaoItem", "SolicitacaoId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstSolicitacaoItem", "SolicitacaoId");
            AddForeignKey("dbo.EstSolicitacaoItem", "SolicitacaoId", "dbo.EstoquePreMovimento", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstSolicitacaoItem", "SolicitacaoId", "dbo.EstoquePreMovimento");
            DropIndex("dbo.EstSolicitacaoItem", new[] { "SolicitacaoId" });
            DropColumn("dbo.EstSolicitacaoItem", "SolicitacaoId");
        }
    }
}
