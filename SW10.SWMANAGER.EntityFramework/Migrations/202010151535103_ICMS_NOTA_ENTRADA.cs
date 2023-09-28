namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ICMS_NOTA_ENTRADA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimentoItem", "ValorICMS", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.EstoquePreMovimentoItem", "PerICMS", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstoquePreMovimentoItem", "PerICMS");
            DropColumn("dbo.EstoquePreMovimentoItem", "ValorICMS");
        }
    }
}
