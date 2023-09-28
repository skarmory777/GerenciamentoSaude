namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Ajuste_tabela_Visitante : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteVisitante", "DataEntrada", c => c.DateTime());
            AddColumn("dbo.AteVisitante", "DataSaida", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.AteVisitante", "DataSaida");
            DropColumn("dbo.AteVisitante", "DataEntrada");
        }
    }
}
