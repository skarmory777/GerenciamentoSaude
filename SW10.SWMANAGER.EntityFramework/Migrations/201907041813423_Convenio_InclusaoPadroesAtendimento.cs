namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Convenio_InclusaoPadroesAtendimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "IsPreencheGuiaAutomaticamente", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisConvenio", "EmpresaPadraoEmergenciaId", c => c.Long());
            AddColumn("dbo.SisConvenio", "MedicoPadraoEmergenciaId", c => c.Long());
            AddColumn("dbo.SisConvenio", "EspecialidadePadraoEmergenciaId", c => c.Long());
            AddColumn("dbo.SisConvenio", "EmpresaPadraoInternacaoId", c => c.Long());
            AddColumn("dbo.SisConvenio", "MedicoPadraoInternacaoId", c => c.Long());
            AddColumn("dbo.SisConvenio", "EspecialidadePadraoInternacaoId", c => c.Long());
            CreateIndex("dbo.SisConvenio", "EmpresaPadraoEmergenciaId");
            CreateIndex("dbo.SisConvenio", "MedicoPadraoEmergenciaId");
            CreateIndex("dbo.SisConvenio", "EspecialidadePadraoEmergenciaId");
            CreateIndex("dbo.SisConvenio", "EmpresaPadraoInternacaoId");
            CreateIndex("dbo.SisConvenio", "MedicoPadraoInternacaoId");
            CreateIndex("dbo.SisConvenio", "EspecialidadePadraoInternacaoId");
            AddForeignKey("dbo.SisConvenio", "EmpresaPadraoEmergenciaId", "dbo.SisEmpresa", "Id");
            AddForeignKey("dbo.SisConvenio", "EmpresaPadraoInternacaoId", "dbo.SisEmpresa", "Id");
            AddForeignKey("dbo.SisConvenio", "EspecialidadePadraoEmergenciaId", "dbo.SisEspecialidade", "Id");
            AddForeignKey("dbo.SisConvenio", "EspecialidadePadraoInternacaoId", "dbo.SisEspecialidade", "Id");
            AddForeignKey("dbo.SisConvenio", "MedicoPadraoEmergenciaId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.SisConvenio", "MedicoPadraoInternacaoId", "dbo.SisMedico", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisConvenio", "MedicoPadraoInternacaoId", "dbo.SisMedico");
            DropForeignKey("dbo.SisConvenio", "MedicoPadraoEmergenciaId", "dbo.SisMedico");
            DropForeignKey("dbo.SisConvenio", "EspecialidadePadraoInternacaoId", "dbo.SisEspecialidade");
            DropForeignKey("dbo.SisConvenio", "EspecialidadePadraoEmergenciaId", "dbo.SisEspecialidade");
            DropForeignKey("dbo.SisConvenio", "EmpresaPadraoInternacaoId", "dbo.SisEmpresa");
            DropForeignKey("dbo.SisConvenio", "EmpresaPadraoEmergenciaId", "dbo.SisEmpresa");
            DropIndex("dbo.SisConvenio", new[] { "EspecialidadePadraoInternacaoId" });
            DropIndex("dbo.SisConvenio", new[] { "MedicoPadraoInternacaoId" });
            DropIndex("dbo.SisConvenio", new[] { "EmpresaPadraoInternacaoId" });
            DropIndex("dbo.SisConvenio", new[] { "EspecialidadePadraoEmergenciaId" });
            DropIndex("dbo.SisConvenio", new[] { "MedicoPadraoEmergenciaId" });
            DropIndex("dbo.SisConvenio", new[] { "EmpresaPadraoEmergenciaId" });
            DropColumn("dbo.SisConvenio", "EspecialidadePadraoInternacaoId");
            DropColumn("dbo.SisConvenio", "MedicoPadraoInternacaoId");
            DropColumn("dbo.SisConvenio", "EmpresaPadraoInternacaoId");
            DropColumn("dbo.SisConvenio", "EspecialidadePadraoEmergenciaId");
            DropColumn("dbo.SisConvenio", "MedicoPadraoEmergenciaId");
            DropColumn("dbo.SisConvenio", "EmpresaPadraoEmergenciaId");
            DropColumn("dbo.SisConvenio", "IsPreencheGuiaAutomaticamente");
        }
    }
}
