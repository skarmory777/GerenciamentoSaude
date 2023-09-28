namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstoqueMovimentoItem_EstoquePreMovimentoItem_ValorIPI_Precisao : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EstoquePreMovimentoItem", "ValorIPI", c => c.Decimal(nullable: false, precision: 18, scale: 10));
            AlterColumn("dbo.EstoqueMovimentoItem", "ValorIPI", c => c.Decimal(nullable: false, precision: 18, scale: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EstoqueMovimentoItem", "ValorIPI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.EstoquePreMovimentoItem", "ValorIPI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
    }
}
