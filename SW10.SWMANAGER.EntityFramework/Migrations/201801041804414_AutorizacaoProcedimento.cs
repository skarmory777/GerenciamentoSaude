namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AutorizacaoProcedimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "ConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "PacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "SolicitanteId", "dbo.SisMedico");
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "SolicitanteId" });
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "PacienteId" });
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "ConvenioId" });
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "FaturamentoItemId" });
            DropTable("dbo.AteAutorizacaoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AutorizacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
            CreateTable(
                "dbo.AteAutorizacaoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SolicitanteId = c.Long(),
                    PacienteId = c.Long(),
                    ConvenioId = c.Long(nullable: false),
                    FaturamentoItemId = c.Long(nullable: false),
                    Senha = c.String(),
                    DataAutorizacao = c.DateTime(),
                    DataSolicitacao = c.DateTime(nullable: false),
                    AutorizadoPor = c.String(),
                    Observacao = c.String(),
                    IsOrtese = c.Boolean(nullable: false),
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
                    { "DynamicFilter_AutorizacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.AteAutorizacaoProcedimento", "FaturamentoItemId");
            CreateIndex("dbo.AteAutorizacaoProcedimento", "ConvenioId");
            CreateIndex("dbo.AteAutorizacaoProcedimento", "PacienteId");
            CreateIndex("dbo.AteAutorizacaoProcedimento", "SolicitanteId");
            AddForeignKey("dbo.AteAutorizacaoProcedimento", "SolicitanteId", "dbo.SisMedico", "Id");
            AddForeignKey("dbo.AteAutorizacaoProcedimento", "PacienteId", "dbo.SisPaciente", "Id");
            AddForeignKey("dbo.AteAutorizacaoProcedimento", "FaturamentoItemId", "dbo.FatItem", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AteAutorizacaoProcedimento", "ConvenioId", "dbo.SisConvenio", "Id", cascadeDelete: true);
        }
    }
}
