namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicionando_relacionamento_atendimento_medicosolicitante_LabResultado_campo_Nic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultado", "SisMedicoSolicitanteId", c => c.Long());
            AddColumn("dbo.LabResultado", "AteAtendimentoId", c => c.Long());
            AddColumn("dbo.LabResultado", "Nic", c => c.Long(nullable: false));
            CreateIndex("dbo.LabResultado", "SisMedicoSolicitanteId");
            CreateIndex("dbo.LabResultado", "AteAtendimentoId");
            AddForeignKey("dbo.LabResultado", "AteAtendimentoId", "dbo.AteAtendimento", "Id");
            AddForeignKey("dbo.LabResultado", "SisMedicoSolicitanteId", "dbo.SisMedico", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LabResultado", "SisMedicoSolicitanteId", "dbo.SisMedico");
            DropForeignKey("dbo.LabResultado", "AteAtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.LabResultado", new[] { "AteAtendimentoId" });
            DropIndex("dbo.LabResultado", new[] { "SisMedicoSolicitanteId" });
            DropColumn("dbo.LabResultado", "Nic");
            DropColumn("dbo.LabResultado", "AteAtendimentoId");
            DropColumn("dbo.LabResultado", "SisMedicoSolicitanteId");
        }
    }
}
