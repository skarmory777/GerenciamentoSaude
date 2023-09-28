namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AltFila_MovSenha : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteFila", "UltimaSenha", c => c.Int(nullable: false));
            AddColumn("dbo.AteFila", "UltimaZera", c => c.DateTime(nullable: false));
            AddColumn("dbo.AteSenhaMovPainel", "IsMostra", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AteSenhaMovPainel", "IsMostra");
            DropColumn("dbo.AteFila", "UltimaZera");
            DropColumn("dbo.AteFila", "UltimaSenha");
        }
    }
}
