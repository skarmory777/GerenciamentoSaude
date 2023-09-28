namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Ajustes_Prescricao_Medica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoMedica", "LiberadoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "DataLiberado", c => c.DateTime());
            AddColumn("dbo.AssPrescricaoMedica", "SuspensoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoMedica", "DataSuspenso", c => c.DateTime());
            AddColumn("dbo.AssPrescricaoItemResposta", "LiberadoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoItemResposta", "DataLiberado", c => c.DateTime());
            AddColumn("dbo.AssPrescricaoItemResposta", "IsAcrescimo", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItemResposta", "AcrescimoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoItemResposta", "DataAcrescimo", c => c.DateTime());
            AddColumn("dbo.AssPrescricaoItemResposta", "IsSuspenso", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssPrescricaoItemResposta", "SuspensoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoItemResposta", "DataSuspenso", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "DataSuspenso");
            DropColumn("dbo.AssPrescricaoItemResposta", "SuspensoUserId");
            DropColumn("dbo.AssPrescricaoItemResposta", "IsSuspenso");
            DropColumn("dbo.AssPrescricaoItemResposta", "DataAcrescimo");
            DropColumn("dbo.AssPrescricaoItemResposta", "AcrescimoUserId");
            DropColumn("dbo.AssPrescricaoItemResposta", "IsAcrescimo");
            DropColumn("dbo.AssPrescricaoItemResposta", "DataLiberado");
            DropColumn("dbo.AssPrescricaoItemResposta", "LiberadoUserId");
            DropColumn("dbo.AssPrescricaoMedica", "DataSuspenso");
            DropColumn("dbo.AssPrescricaoMedica", "SuspensoUserId");
            DropColumn("dbo.AssPrescricaoMedica", "DataLiberado");
            DropColumn("dbo.AssPrescricaoMedica", "LiberadoUserId");
        }
    }
}
