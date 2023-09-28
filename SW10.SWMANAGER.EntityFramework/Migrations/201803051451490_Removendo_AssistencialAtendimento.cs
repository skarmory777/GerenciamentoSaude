namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Removendo_AssistencialAtendimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssistencialAtendimento", "SisConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.AssistencialAtendimento", "SisMedicoId", "dbo.SisMedico");
            DropForeignKey("dbo.AssistencialAtendimento", "SisPacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.AssistencialAtendimento", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.AssistencialAtendimento", "SisEmpresaId", "dbo.SisEmpresa");
            DropIndex("dbo.AssistencialAtendimento", new[] { "SisPacienteId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "SisMedicoId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "SisConvenioId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "SisEmpresaId" });
            DropIndex("dbo.AssistencialAtendimento", new[] { "SisUnidadeOrganizacionalId" });
            DropTable("dbo.AssistencialAtendimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AssistencialAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.AssistencialAtendimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataRegistro = c.DateTime(nullable: false),
                    SisPacienteId = c.Long(),
                    SisMedicoId = c.Long(),
                    SisConvenioId = c.Long(),
                    SisEmpresaId = c.Long(),
                    SisUnidadeOrganizacionalId = c.Long(),
                    TipoGuia = c.String(),
                    NumeroGuia = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AssistencialAtendimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.AssistencialAtendimento", "SisUnidadeOrganizacionalId");
            CreateIndex("dbo.AssistencialAtendimento", "SisEmpresaId");
            CreateIndex("dbo.AssistencialAtendimento", "SisConvenioId");
            CreateIndex("dbo.AssistencialAtendimento", "SisMedicoId");
            CreateIndex("dbo.AssistencialAtendimento", "SisPacienteId");
            AddForeignKey("dbo.AssistencialAtendimento", "SisEmpresaId", "dbo.SisEmpresa", "Id");
            AddForeignKey("dbo.AssistencialAtendimento", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
            AddForeignKey("dbo.AssistencialAtendimento", "SisPacienteId", "dbo.SisPaciente", "Id");
            AddForeignKey("dbo.AssistencialAtendimento", "SisMedicoId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.AssistencialAtendimento", "SisConvenioId", "dbo.SisConvenio", "Id");
        }
    }
}
