namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atendimento_Data_Tomada_Decisao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "DataTomadaDecisao", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "DataTomadaDecisao");
        }
    }
}
