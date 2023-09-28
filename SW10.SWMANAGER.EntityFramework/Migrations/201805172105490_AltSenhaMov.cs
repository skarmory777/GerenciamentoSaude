namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AltSenhaMov : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AtSenhaMov", "DataHora", c => c.DateTime(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AtSenhaMov", "DataHora");
        }
    }
}
