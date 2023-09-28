namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ConfiguracaoPrescricaoItem_Alterações : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssConfiguracaoPrescricaoItem", "Options", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssConfiguracaoPrescricaoItem", "Options");
        }
    }
}
