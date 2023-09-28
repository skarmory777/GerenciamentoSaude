namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTabelasDisponibilidade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAgendamentoConsultaMedicoDisponibilidade", "EmpresaId", c => c.Long());
            AddColumn("dbo.AgendamentoSalaCirurgicaDisponibilidades", "EmpresaId", c => c.Long());
            CreateIndex("dbo.AteAgendamentoConsultaMedicoDisponibilidade", "EmpresaId");
            CreateIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", "EmpresaId");
            AddForeignKey("dbo.AteAgendamentoConsultaMedicoDisponibilidade", "EmpresaId", "dbo.SisEmpresa", "Id");
            AddForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "EmpresaId", "dbo.SisEmpresa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AgendamentoSalaCirurgicaDisponibilidades", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.AteAgendamentoConsultaMedicoDisponibilidade", "EmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.AgendamentoSalaCirurgicaDisponibilidades", new[] { "EmpresaId" });
            DropIndex("dbo.AteAgendamentoConsultaMedicoDisponibilidade", new[] { "EmpresaId" });
            DropColumn("dbo.AgendamentoSalaCirurgicaDisponibilidades", "EmpresaId");
            DropColumn("dbo.AteAgendamentoConsultaMedicoDisponibilidade", "EmpresaId");
        }
    }
}
