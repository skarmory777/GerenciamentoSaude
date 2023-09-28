namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoEstoquePreMovimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "PacienteId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "MedicoSolcitanteId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "UnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.EstoquePreMovimento", "Observacao", c => c.String());
            CreateIndex("dbo.EstoquePreMovimento", "PacienteId");
            CreateIndex("dbo.EstoquePreMovimento", "MedicoSolcitanteId");
            CreateIndex("dbo.EstoquePreMovimento", "UnidadeOrganizacionalId");
            AddForeignKey("dbo.EstoquePreMovimento", "MedicoSolcitanteId", "dbo.Medico", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "PacienteId", "dbo.Paciente", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.EstoquePreMovimento", "PacienteId", "dbo.Paciente");
            DropForeignKey("dbo.EstoquePreMovimento", "MedicoSolcitanteId", "dbo.Medico");
            DropIndex("dbo.EstoquePreMovimento", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "MedicoSolcitanteId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "PacienteId" });
            DropColumn("dbo.EstoquePreMovimento", "Observacao");
            DropColumn("dbo.EstoquePreMovimento", "UnidadeOrganizacionalId");
            DropColumn("dbo.EstoquePreMovimento", "MedicoSolcitanteId");
            DropColumn("dbo.EstoquePreMovimento", "PacienteId");
        }
    }
}
