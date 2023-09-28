namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatConta_Advinhando_Campos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatConta", "SisPacienteId", c => c.Long());
            AddColumn("dbo.FatConta", "SisConvenioId", c => c.Long());
            AddColumn("dbo.FatConta", "SisEmpresaId", c => c.Long());
            AddColumn("dbo.FatConta", "SisAtendimentoId", c => c.Long());
            AddColumn("dbo.FatConta", "SisUnidadeOrganizacionalId", c => c.Long());
            AddColumn("dbo.FatConta", "DataIncio", c => c.DateTime());
            AddColumn("dbo.FatConta", "DataFim", c => c.DateTime());
            CreateIndex("dbo.FatConta", "SisPacienteId");
            CreateIndex("dbo.FatConta", "SisConvenioId");
            CreateIndex("dbo.FatConta", "SisEmpresaId");
            CreateIndex("dbo.FatConta", "SisAtendimentoId");
            CreateIndex("dbo.FatConta", "SisUnidadeOrganizacionalId");
            AddForeignKey("dbo.FatConta", "SisAtendimentoId", "dbo.Atendimento", "Id");
            AddForeignKey("dbo.FatConta", "SisConvenioId", "dbo.Convenio", "Id");
            AddForeignKey("dbo.FatConta", "SisEmpresaId", "dbo.Empresa", "Id");
            AddForeignKey("dbo.FatConta", "SisPacienteId", "dbo.Paciente", "Id");
            AddForeignKey("dbo.FatConta", "SisUnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatConta", "SisUnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.FatConta", "SisPacienteId", "dbo.Paciente");
            DropForeignKey("dbo.FatConta", "SisEmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.FatConta", "SisConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.FatConta", "SisAtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.FatConta", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.FatConta", new[] { "SisAtendimentoId" });
            DropIndex("dbo.FatConta", new[] { "SisEmpresaId" });
            DropIndex("dbo.FatConta", new[] { "SisConvenioId" });
            DropIndex("dbo.FatConta", new[] { "SisPacienteId" });
            DropColumn("dbo.FatConta", "DataFim");
            DropColumn("dbo.FatConta", "DataIncio");
            DropColumn("dbo.FatConta", "SisUnidadeOrganizacionalId");
            DropColumn("dbo.FatConta", "SisAtendimentoId");
            DropColumn("dbo.FatConta", "SisEmpresaId");
            DropColumn("dbo.FatConta", "SisConvenioId");
            DropColumn("dbo.FatConta", "SisPacienteId");
        }
    }
}
