namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GuiaNovoModeloComPNG : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guia", "ModeloPDFMimeType", c => c.String());
            AddColumn("dbo.Guia", "ModeloPNG", c => c.Binary());
            AddColumn("dbo.Guia", "ModeloPNGMimeType", c => c.String());
            DropColumn("dbo.Guia", "ModeloMimeType");
        }

        public override void Down()
        {
            AddColumn("dbo.Guia", "ModeloMimeType", c => c.String());
            DropColumn("dbo.Guia", "ModeloPNGMimeType");
            DropColumn("dbo.Guia", "ModeloPNG");
            DropColumn("dbo.Guia", "ModeloPDFMimeType");
        }
    }
}
