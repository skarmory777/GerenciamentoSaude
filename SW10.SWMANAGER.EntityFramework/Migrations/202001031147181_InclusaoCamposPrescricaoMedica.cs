namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InclusaoCamposPrescricaoMedica : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItemResposta", "ReativadoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoItemResposta", "DataReativado", c => c.DateTime());
            DropForeignKey("dbo.AssPrescricaoItemResposta", "FK_dbo.AssFormTipoResposta_dbo.AssPrescricaoItemStatus_AssPrescricaoItemStatusId");
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "DataReativado");
            DropColumn("dbo.AssPrescricaoItemResposta", "ReativadoUserId");
        }
    }
}
