namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atendimento_Usuario_Tomada_Decisao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "UsuarioTomadaDecisao", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "UsuarioTomadaDecisao");
        }
    }
}
