namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Acerto_Bairro : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SisBairro", "Capital");
        }

        public override void Down()
        {
            AddColumn("dbo.SisBairro", "Capital", c => c.Boolean(nullable: false));
        }
    }
}
