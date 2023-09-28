namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ParametrizacaoColetaAutomatica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisParametrizacoes", "IsHabilitaAssistencialColetaAutomatica", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisParametrizacoes", "IsHabilitaAssistencialColetaAutomatica");
        }
    }
}
