namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AjusteAntimicrobianoCultura : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoAntimicrobianosCulturas", "StatusResultado", c => c.Boolean());
            DropColumn("dbo.AssSolicitacaoAntimicrobianosResultados", "StatusResultado");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssSolicitacaoAntimicrobianosResultados", "StatusResultado", c => c.Boolean());
            DropColumn("dbo.AssSolicitacaoAntimicrobianosCulturas", "StatusResultado");
        }
    }
}
