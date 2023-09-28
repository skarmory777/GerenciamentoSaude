namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoDisparoAtivoEmDisparoDeMensagem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisDisparoDeMensagem", "DisparoAtivo", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisDisparoDeMensagem", "DisparoAtivo");
        }
    }
}
