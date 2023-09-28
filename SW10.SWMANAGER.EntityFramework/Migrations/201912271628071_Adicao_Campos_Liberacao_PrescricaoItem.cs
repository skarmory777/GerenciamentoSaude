namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Adicao_Campos_Liberacao_PrescricaoItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItemResposta", "AprovadoUserId", c => c.Long());
            AddColumn("dbo.AssPrescricaoItemResposta", "DataAprovado", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "DataAprovado");
            DropColumn("dbo.AssPrescricaoItemResposta", "AprovadoUserId");
        }
    }
}
