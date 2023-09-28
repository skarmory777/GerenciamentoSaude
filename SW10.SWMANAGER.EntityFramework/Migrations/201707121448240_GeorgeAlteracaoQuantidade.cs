namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoQuantidade : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EstoqueMovimentoItem", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EstoquePreMovimentoItem", "Quantidade", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            AlterColumn("dbo.EstoquePreMovimentoItem", "Quantidade", c => c.Long(nullable: false));
            AlterColumn("dbo.EstoqueMovimentoItem", "Quantidade", c => c.Long(nullable: false));
        }
    }
}
