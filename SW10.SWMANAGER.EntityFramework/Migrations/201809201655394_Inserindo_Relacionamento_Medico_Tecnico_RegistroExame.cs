namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inserindo_Relacionamento_Medico_Tecnico_RegistroExame : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LauMovimento", "SisMedicoSolicitanteId", c => c.Long());
            AddColumn("dbo.LauMovimento", "LabTecnicoId", c => c.Long());
            AddColumn("dbo.LauMovimento", "Crm", c => c.String());
            CreateIndex("dbo.LauMovimento", "SisMedicoSolicitanteId");
            CreateIndex("dbo.LauMovimento", "LabTecnicoId");
            AddForeignKey("dbo.LauMovimento", "SisMedicoSolicitanteId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.LauMovimento", "LabTecnicoId", "dbo.LabTecnico", "Id");
            DropColumn("dbo.LauMovimento", "Tecnico");
        }

        public override void Down()
        {
            AddColumn("dbo.LauMovimento", "Tecnico", c => c.String());
            DropForeignKey("dbo.LauMovimento", "LabTecnicoId", "dbo.LabTecnico");
            DropForeignKey("dbo.LauMovimento", "SisMedicoSolicitanteId", "dbo.SisMedico");
            DropIndex("dbo.LauMovimento", new[] { "LabTecnicoId" });
            DropIndex("dbo.LauMovimento", new[] { "SisMedicoSolicitanteId" });
            DropColumn("dbo.LauMovimento", "Crm");
            DropColumn("dbo.LauMovimento", "LabTecnicoId");
            DropColumn("dbo.LauMovimento", "SisMedicoSolicitanteId");
        }
    }
}
