namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAteAutorizacaoProcedimento : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AutorizacaoProcedimento", newName: "AteAutorizacaoProcedimento");
            AddColumn("dbo.AteAutorizacaoProcedimento", "IsOrtese", c => c.Boolean(nullable: false));
            DropColumn("dbo.AteAutorizacaoProcedimento", "IsOstese");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAutorizacaoProcedimento", "IsOstese", c => c.Boolean(nullable: false));
            DropColumn("dbo.AteAutorizacaoProcedimento", "IsOrtese");
            RenameTable(name: "dbo.AteAutorizacaoProcedimento", newName: "AutorizacaoProcedimento");
        }
    }
}
