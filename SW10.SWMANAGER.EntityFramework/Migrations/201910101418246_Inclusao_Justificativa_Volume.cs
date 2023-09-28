namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_Justificativa_Volume : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssSolicitacaoExame", "Justificativa", c => c.String());
            AddColumn("dbo.AssPrescricaoItemResposta", "VolumeDiluente", c => c.Double());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "VolumeDiluente");
            DropColumn("dbo.AssSolicitacaoExame", "Justificativa");
        }
    }
}
