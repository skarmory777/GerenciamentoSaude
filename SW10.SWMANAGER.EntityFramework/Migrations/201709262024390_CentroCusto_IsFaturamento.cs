namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CentroCusto_IsFaturamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CentroCusto", "IsFaturamento", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CentroCusto", "IsFaturamento");
        }
    }
}
