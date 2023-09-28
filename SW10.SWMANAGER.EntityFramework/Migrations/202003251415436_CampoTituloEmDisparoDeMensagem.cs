namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CampoTituloEmDisparoDeMensagem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisDisparoDeMensagem", "Titulo", c => c.String());
            AddColumn("dbo.SisDisparoDeMensagemItem", "Titulo", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisDisparoDeMensagemItem", "Titulo");
            DropColumn("dbo.SisDisparoDeMensagem", "Titulo");
        }
    }
}
