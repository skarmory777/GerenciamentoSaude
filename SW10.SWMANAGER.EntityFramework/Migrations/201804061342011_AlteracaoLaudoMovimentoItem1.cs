namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoLaudoMovimentoItem1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimentoItem", "IsIndicativo", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "IsSolicatacaoRevisao", c => c.Boolean(nullable: false));
            AddColumn("dbo.LauMovimentoItem", "ComentarioLaudo", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "JustificativaContraste", c => c.String());
            AddColumn("dbo.LauMovimentoItem", "MotivoDiscordancia", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.LauMovimentoItem", "MotivoDiscordancia");
            DropColumn("dbo.LauMovimentoItem", "JustificativaContraste");
            DropColumn("dbo.LauMovimentoItem", "ComentarioLaudo");
            DropColumn("dbo.LauMovimentoItem", "IsSolicatacaoRevisao");
            DropColumn("dbo.LauMovimentoItem", "IsIndicativo");
        }
    }
}
