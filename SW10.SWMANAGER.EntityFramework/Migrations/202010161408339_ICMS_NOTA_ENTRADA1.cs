namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ICMS_NOTA_ENTRADA1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoqueMovimentoItem", "PerICMS", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.EstoqueMovimentoItem", "ValorICMS", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstoqueMovimentoItem", "ValorICMS");
            DropColumn("dbo.EstoqueMovimentoItem", "PerICMS");
        }
    }
}
