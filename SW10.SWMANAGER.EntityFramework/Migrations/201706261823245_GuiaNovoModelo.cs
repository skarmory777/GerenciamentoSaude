namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GuiaNovoModelo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guia", "ModeloPdf", c => c.Binary());
            AddColumn("dbo.Guia", "ModeloMimeType", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Guia", "ModeloMimeType");
            DropColumn("dbo.Guia", "ModeloPdf");
        }
    }
}
