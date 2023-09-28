namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DocRotulo_Modulo_Status : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocRotulo", "IsModulo", c => c.Boolean(nullable: false));
            AddColumn("dbo.DocRotulo", "IsStatus", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.DocRotulo", "IsStatus");
            DropColumn("dbo.DocRotulo", "IsModulo");
        }
    }
}
