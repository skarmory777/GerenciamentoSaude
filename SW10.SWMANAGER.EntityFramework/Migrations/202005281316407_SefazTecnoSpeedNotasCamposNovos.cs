namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SefazTecnoSpeedNotasCamposNovos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sefaz_TecnoSpeed_Notas", "LastAttemptSefazTecnospeed", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sefaz_TecnoSpeed_Notas", "LastAttemptSefazTecnospeed");
        }
    }
}
