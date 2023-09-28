namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoPrecisao2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EstoqueMovimentoItem", "CustoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }

        public override void Down()
        {
            AlterColumn("dbo.EstoqueMovimentoItem", "CustoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
