namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Acerto_SisTarefa : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SisTarefa", "Ordem", c => c.Decimal(precision: 18, scale: 2));
        }

        public override void Down()
        {
            AlterColumn("dbo.SisTarefa", "Ordem", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
