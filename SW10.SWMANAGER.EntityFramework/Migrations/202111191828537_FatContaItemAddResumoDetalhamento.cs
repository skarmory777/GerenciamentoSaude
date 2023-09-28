namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FatContaItemAddResumoDetalhamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "ResumoDetalhamento", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FatContaItem", "ResumoDetalhamento");
        }
    }
}
