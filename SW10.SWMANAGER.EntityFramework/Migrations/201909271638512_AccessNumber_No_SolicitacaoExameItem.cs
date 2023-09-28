namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccessNumber_No_SolicitacaoExameItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoExameItem", "AccessNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssSolicitacaoExameItem", "AccessNumber");
        }
    }
}
