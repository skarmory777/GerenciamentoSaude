namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AjustesFormularioDinamico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "Orientation", c => c.String());
            AddColumn("dbo.SisFormColConfig", "PrependText", c => c.String());
            AddColumn("dbo.SisFormColConfig", "AppendText", c => c.String());
            AddColumn("dbo.SisFormColConfig", "Offset", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisFormColConfig", "Offset");
            DropColumn("dbo.SisFormColConfig", "AppendText");
            DropColumn("dbo.SisFormColConfig", "PrependText");
            DropColumn("dbo.SisFormColConfig", "Orientation");
        }
    }
}
