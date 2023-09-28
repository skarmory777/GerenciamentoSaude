namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Faturamento_Alteracoes_Campos_Convenio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "HabilitaAuditoriaInterna", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "HabilitaAuditoriaExterna", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "HabilitaEntrega", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SisConvenio", "HabilitaEntrega");
            DropColumn("dbo.SisConvenio", "HabilitaAuditoriaExterna");
            DropColumn("dbo.SisConvenio", "HabilitaAuditoriaInterna");
        }
    }
}
