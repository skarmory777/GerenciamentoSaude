namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteCustoUnitarioCasas : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EstoqueMovimentoItem", "CustoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.EstoquePreMovimentoItem", "CustoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EstoquePreMovimentoItem", "CustoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("dbo.EstoqueMovimentoItem", "CustoUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }
    }
}
