namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BalancoHidricoAlteracoes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteBalancoHidricoItens", "Enteral", c => c.String());
            AddColumn("dbo.AteBalancoHidricoItens", "Hd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteBalancoHidricoItens", "Hd");
            DropColumn("dbo.AteBalancoHidricoItens", "Enteral");
        }
    }
}
