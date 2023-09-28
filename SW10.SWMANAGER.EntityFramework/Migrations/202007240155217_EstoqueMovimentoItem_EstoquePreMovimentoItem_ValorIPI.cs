namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EstoqueMovimentoItem_EstoquePreMovimentoItem_ValorIPI : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimentoItem", "ValorIPI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.EstoqueMovimentoItem", "ValorIPI", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstoqueMovimentoItem", "ValorIPI");
            DropColumn("dbo.EstoquePreMovimentoItem", "ValorIPI");
        }
    }
}
