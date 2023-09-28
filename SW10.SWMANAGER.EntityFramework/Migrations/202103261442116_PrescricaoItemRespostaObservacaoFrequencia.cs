namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrescricaoItemRespostaObservacaoFrequencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssPrescricaoItemResposta", "ObsFrequencia", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AssPrescricaoItemResposta", "ObsFrequencia");
        }
    }
}
