namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FaturamentoAlteracoesTaxa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatTaxa", "Nivel", c => c.Long());
            AddColumn("dbo.FatTaxa", "IsIncidePorte", c => c.Boolean(nullable: false));
            AddColumn("dbo.FatTaxa", "IsIncidePrecoItem", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FatTaxa", "IsIncidePrecoItem");
            DropColumn("dbo.FatTaxa", "IsIncidePorte");
            DropColumn("dbo.FatTaxa", "Nivel");
        }
    }
}
