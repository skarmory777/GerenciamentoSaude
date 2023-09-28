namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Laboratorio_Add_Local_Material : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabMaterial", "IsDescriminaLocal", c => c.Boolean(nullable: false));
            AddColumn("dbo.LabResultadoExame", "MaterialDescricaoLocal", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LabResultadoExame", "MaterialDescricaoLocal");
            DropColumn("dbo.LabMaterial", "IsDescriminaLocal");
        }
    }
}
