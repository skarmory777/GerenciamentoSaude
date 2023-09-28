namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_IntervaloGuiasConvenio : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisIntervaloGuiasConvenio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(nullable: false),
                    EmpresaId = c.Long(nullable: false),
                    FaturamentoGuiaId = c.Long(nullable: false),
                    Inicio = c.String(),
                    Final = c.String(),
                    NumeroGuiasParaAviso = c.Int(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_IntervaloGuiasConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId, cascadeDelete: true)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId, cascadeDelete: true)
                .ForeignKey("dbo.FatGuia", t => t.FaturamentoGuiaId, cascadeDelete: true)
                .Index(t => t.ConvenioId)
                .Index(t => t.EmpresaId)
                .Index(t => t.FaturamentoGuiaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisIntervaloGuiasConvenio", "FaturamentoGuiaId", "dbo.FatGuia");
            DropForeignKey("dbo.SisIntervaloGuiasConvenio", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.SisIntervaloGuiasConvenio", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.SisIntervaloGuiasConvenio", new[] { "FaturamentoGuiaId" });
            DropIndex("dbo.SisIntervaloGuiasConvenio", new[] { "EmpresaId" });
            DropIndex("dbo.SisIntervaloGuiasConvenio", new[] { "ConvenioId" });
            DropTable("dbo.SisIntervaloGuiasConvenio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_IntervaloGuiasConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
