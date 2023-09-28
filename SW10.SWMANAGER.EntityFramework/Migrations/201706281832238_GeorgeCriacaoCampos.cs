namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeCriacaoCampos : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "MedicoId", "dbo.Medico");
            DropIndex("dbo.EstoquePreMovimento", new[] { "MedicoId" });
            AddColumn("dbo.EstoqueMovimento", "PacienteId", c => c.Long());
            AddColumn("dbo.EstoqueMovimento", "MedicoSolcitanteId", c => c.Long());
            AddColumn("dbo.EstoqueMovimento", "UnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.EstoqueMovimento", "Observacao", c => c.String());
            AddColumn("dbo.EstoqueMovimento", "AtendimentoId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "PacienteId");
            CreateIndex("dbo.EstoqueMovimento", "MedicoSolcitanteId");
            CreateIndex("dbo.EstoqueMovimento", "UnidadeOrganizacionalId");
            CreateIndex("dbo.EstoqueMovimento", "AtendimentoId");
            AddForeignKey("dbo.EstoqueMovimento", "AtendimentoId", "dbo.Atendimento", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "MedicoSolcitanteId", "dbo.Medico", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "PacienteId", "dbo.Paciente", "Id");
            AddForeignKey("dbo.EstoqueMovimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id");
            DropColumn("dbo.EstoquePreMovimento", "MedicoId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstoquePreMovimento", "MedicoId", c => c.Long());
            DropForeignKey("dbo.EstoqueMovimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.EstoqueMovimento", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.EstoqueMovimento", "MedicoSolcitanteId", "dbo.Medico");
            DropForeignKey("dbo.EstoqueMovimento", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.EstoqueMovimento", new[] { "AtendimentoId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "MedicoSolcitanteId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "PacienteId" });
            DropColumn("dbo.EstoqueMovimento", "AtendimentoId");
            DropColumn("dbo.EstoqueMovimento", "Observacao");
            DropColumn("dbo.EstoqueMovimento", "UnidadeOrganizacionalId");
            DropColumn("dbo.EstoqueMovimento", "MedicoSolcitanteId");
            DropColumn("dbo.EstoqueMovimento", "PacienteId");
            CreateIndex("dbo.EstoquePreMovimento", "MedicoId");
            AddForeignKey("dbo.EstoquePreMovimento", "MedicoId", "dbo.Medico", "Id");
        }
    }
}
