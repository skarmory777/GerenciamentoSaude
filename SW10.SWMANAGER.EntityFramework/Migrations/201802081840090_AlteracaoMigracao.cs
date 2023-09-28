namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoMigracao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinQuitacao", "ValorQuitacao", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.FinQuitacao", "ValorEfetivo", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.FinQuitacao", "ValorEfetivo");
            DropColumn("dbo.FinQuitacao", "ValorQuitacao");
        }
    }
}
