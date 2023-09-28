namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeCampoIPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoqueMovimentoItem", "PerIPI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.EstoquePreMovimentoItem", "PerIPI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.EstoquePreMovimentoItem", "PerIPI");
            DropColumn("dbo.EstoqueMovimentoItem", "PerIPI");
        }
    }
}
