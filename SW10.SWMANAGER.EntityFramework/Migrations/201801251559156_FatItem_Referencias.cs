namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatItem_Referencias : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "Referencia", c => c.String());
            AddColumn("dbo.FatItem", "ReferenciaSihSus", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FatItem", "ReferenciaSihSus");
            DropColumn("dbo.FatItem", "Referencia");
        }
    }
}
