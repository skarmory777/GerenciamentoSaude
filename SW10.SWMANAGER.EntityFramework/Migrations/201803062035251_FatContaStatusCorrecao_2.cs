namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatContaStatusCorrecao_2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.FatConta", "StatusEntrega");
        }

        public override void Down()
        {
            AddColumn("dbo.FatConta", "StatusEntrega", c => c.Int(nullable: false));
        }
    }
}
