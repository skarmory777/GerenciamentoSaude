namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Alteracoes_banco_pos_correcoes_maquina_Novaes : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.AgendamentoConsultaMedicoDisponibilidade", newName: "AteAgendamentoConsultaMedicoDisponibilidade");
            //RenameTable(name: "dbo.Intervalo", newName: "AteIntervalo");
            //DropForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio");
            //DropIndex("dbo.LoteValidade", new[] { "EstEstoqueLaboratorioId" });
            //RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "MedicoId", newName: "SisMedicoId");
            //RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "MedicoEspecialidadeId", newName: "SisMedicoEspecialidadeId");
            //RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IntervaloId", newName: "AteIntervaloId");
            //RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_MedicoId", newName: "IX_SisMedicoId");
            //RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_MedicoEspecialidadeId", newName: "IX_SisMedicoEspecialidadeId");
            //RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_IntervaloId", newName: "IX_AteIntervaloId");
            //AddColumn("dbo.AteVisitante", "Foto", c => c.Binary());
            //AddColumn("dbo.AteVisitante", "FotoMimeType", c => c.String());
            //AlterColumn("dbo.AteVisitante", "Nome", c => c.String(maxLength: 100));
            //AlterColumn("dbo.AteVisitante", "Documento", c => c.String(maxLength: 14));
            //AlterColumn("dbo.LoteValidade", "EstEstoqueLaboratorioId", c => c.Long());
            //CreateIndex("dbo.LoteValidade", "EstEstoqueLaboratorioId");
            //AddForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio", "Id");
        }

        public override void Down()
        {
            //DropForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio");
            //DropIndex("dbo.LoteValidade", new[] { "EstEstoqueLaboratorioId" });
            //AlterColumn("dbo.LoteValidade", "EstEstoqueLaboratorioId", c => c.Long(nullable: false));
            //AlterColumn("dbo.AteVisitante", "Documento", c => c.String());
            //AlterColumn("dbo.AteVisitante", "Nome", c => c.String());
            //DropColumn("dbo.AteVisitante", "FotoMimeType");
            //DropColumn("dbo.AteVisitante", "Foto");
            //RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_AteIntervaloId", newName: "IX_IntervaloId");
            //RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_SisMedicoEspecialidadeId", newName: "IX_MedicoEspecialidadeId");
            //RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_SisMedicoId", newName: "IX_MedicoId");
            //RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "AteIntervaloId", newName: "IntervaloId");
            //RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "SisMedicoEspecialidadeId", newName: "MedicoEspecialidadeId");
            //RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "SisMedicoId", newName: "MedicoId");
            //CreateIndex("dbo.LoteValidade", "EstEstoqueLaboratorioId");
            //AddForeignKey("dbo.LoteValidade", "EstEstoqueLaboratorioId", "dbo.EstLaboratorio", "Id", cascadeDelete: true);
            //RenameTable(name: "dbo.AteIntervalo", newName: "Intervalo");
            //RenameTable(name: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", newName: "AgendamentoConsultaMedicoDisponibilidade");
        }
    }
}
