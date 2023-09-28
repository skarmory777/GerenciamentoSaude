namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjustesPrescricaoItemResposta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItemResposta", "DataAgrupamento", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "DataAgrupamento");
        }
    }
}
