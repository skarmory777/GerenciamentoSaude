namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Visitante : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AteVisitante", "DataEntrada");
            DropColumn("dbo.AteVisitante", "DataSaida");
        }

        public override void Down()
        {
            AddColumn("dbo.AteVisitante", "DataSaida", c => c.DateTime(nullable: false));
            AddColumn("dbo.AteVisitante", "DataEntrada", c => c.DateTime(nullable: false));
        }
    }
}
