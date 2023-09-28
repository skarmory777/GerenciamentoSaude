namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CriandoValorImpostosEmFinQuitacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinQuitacao", "ValorImpostos", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FinQuitacao", "ValorImpostos");
        }
    }
}
