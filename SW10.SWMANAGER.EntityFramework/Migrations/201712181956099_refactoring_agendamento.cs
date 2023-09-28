namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class refactoring_agendamento : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AgendamentoConsultaMedicoDisponibilidade", newName: "AteAgendamentoConsultaMedicoDisponibilidade");
            RenameTable(name: "dbo.Intervalo", newName: "AteIntervalo");
            RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "MedicoId", newName: "SisMedicoId");
            RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "MedicoEspecialidadeId", newName: "SisMedicoEspecialidadeId");
            RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IntervaloId", newName: "AteIntervaloId");
            RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_MedicoId", newName: "IX_SisMedicoId");
            RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_MedicoEspecialidadeId", newName: "IX_SisMedicoEspecialidadeId");
            RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_IntervaloId", newName: "IX_AteIntervaloId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_AteIntervaloId", newName: "IX_IntervaloId");
            RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_SisMedicoEspecialidadeId", newName: "IX_MedicoEspecialidadeId");
            RenameIndex(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "IX_SisMedicoId", newName: "IX_MedicoId");
            RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "AteIntervaloId", newName: "IntervaloId");
            RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "SisMedicoEspecialidadeId", newName: "MedicoEspecialidadeId");
            RenameColumn(table: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", name: "SisMedicoId", newName: "MedicoId");
            RenameTable(name: "dbo.AteIntervalo", newName: "Intervalo");
            RenameTable(name: "dbo.AteAgendamentoConsultaMedicoDisponibilidade", newName: "AgendamentoConsultaMedicoDisponibilidade");
        }
    }
}
