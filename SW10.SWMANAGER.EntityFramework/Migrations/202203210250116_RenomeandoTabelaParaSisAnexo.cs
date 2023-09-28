namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenomeandoTabelaParaSisAnexo : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Anexo", newName: "SisAnexo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.SisAnexo", newName: "Anexo");
        }
    }
}
