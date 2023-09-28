namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Guia_CamposNovoModelo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Guia", "CamposJson", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.Guia", "CamposJson");
        }
    }
}
