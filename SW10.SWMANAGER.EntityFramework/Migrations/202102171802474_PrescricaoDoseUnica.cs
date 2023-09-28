namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrescricaoDoseUnica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssDivisao", "IsDoseUnica", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItemResposta", "DoseUnica", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "DoseUnica");
            DropColumn("dbo.AssDivisao", "IsDoseUnica");
        }
    }
}
