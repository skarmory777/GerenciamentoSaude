namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class alteracoes_visitante : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteVisitante", "IsVisitante", c => c.Boolean(nullable: false));
            DropColumn("dbo.AteVisitante", "IsVisitandte");
        }

        public override void Down()
        {
            AddColumn("dbo.AteVisitante", "IsVisitandte", c => c.Boolean(nullable: false));
            DropColumn("dbo.AteVisitante", "IsVisitante");
        }
    }
}
