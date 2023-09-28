namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResultadoExameTecnicoResponsavel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoExame", "TecnicoColetaId", c => c.Long());
            CreateIndex("dbo.LabResultadoExame", "TecnicoColetaId");
            AddForeignKey("dbo.LabResultadoExame", "TecnicoColetaId", "dbo.LabTecnico", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LabResultadoExame", "TecnicoColetaId", "dbo.LabTecnico");
            DropIndex("dbo.LabResultadoExame", new[] { "TecnicoColetaId" });
            DropColumn("dbo.LabResultadoExame", "TecnicoColetaId");
        }
    }
}
