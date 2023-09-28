namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracao_SolicitacaoAntimicrobiano_Resultado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoAntimicrobianosResultados", "StatusResultado", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssSolicitacaoAntimicrobianosResultados", "StatusResultado");
        }
    }
}
