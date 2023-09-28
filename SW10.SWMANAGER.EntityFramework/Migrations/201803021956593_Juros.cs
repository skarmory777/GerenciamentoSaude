namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Juros : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinLancamentoQuitacao", "Juros", c => c.Decimal(precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.FinLancamentoQuitacao", "Juros");
        }
    }
}
