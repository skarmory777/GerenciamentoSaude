namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessNumber_No_LaudoMovimentoItemDto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimentoItem", "AccessNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LauMovimentoItem", "AccessNumber");
        }
    }
}
