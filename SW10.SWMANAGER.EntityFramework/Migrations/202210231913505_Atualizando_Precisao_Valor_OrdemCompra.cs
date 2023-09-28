namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atualizando_Precisao_Valor_OrdemCompra : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 5));
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorTotal", c => c.Decimal(nullable: false, precision: 18, scale: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorTotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.CmpOrdemCompraItem", "ValorUnitario", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
