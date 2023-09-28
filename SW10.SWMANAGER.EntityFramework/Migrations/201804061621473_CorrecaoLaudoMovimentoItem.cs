namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CorrecaoLaudoMovimentoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimentoItem", "IsSolicitacaoRevisao", c => c.Boolean(nullable: false));
            DropColumn("dbo.LauMovimentoItem", "IsSolicatacaoRevisao");
        }

        public override void Down()
        {
            AddColumn("dbo.LauMovimentoItem", "IsSolicatacaoRevisao", c => c.Boolean(nullable: false));
            DropColumn("dbo.LauMovimentoItem", "IsSolicitacaoRevisao");
        }
    }
}
