namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Campo_Ordem_em_SisTarefa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisTarefa", "Ordem", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.SisTarefa", "Ordem");
        }
    }
}
