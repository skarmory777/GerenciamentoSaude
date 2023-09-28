namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoTotalTransporteBalancoHidrico : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteBalancoHidricoItens", "TotalTransporte", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteBalancoHidricoItens", "TotalTransporte");
        }
    }
}
