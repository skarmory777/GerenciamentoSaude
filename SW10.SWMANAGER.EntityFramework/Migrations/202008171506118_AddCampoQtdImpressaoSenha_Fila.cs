namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCampoQtdImpressaoSenha_Fila : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteFila", "QtdImpressaoSenha", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteFila", "QtdImpressaoSenha");
        }
    }
}
