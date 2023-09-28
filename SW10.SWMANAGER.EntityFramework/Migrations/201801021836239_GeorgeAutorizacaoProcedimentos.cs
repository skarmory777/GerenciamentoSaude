namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class GeorgeAutorizacaoProcedimentos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AutorizacaoProcedimento",
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
                    IsOstese = c.Boolean(nullable: false),
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId, cascadeDelete: true)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: true)
                .ForeignKey("dbo.SisPaciente", t => t.PacienteId)
                .ForeignKey("dbo.SisMedico", t => t.SolicitanteId)
                .Index(t => t.SolicitanteId)
                .Index(t => t.PacienteId)
                .Index(t => t.ConvenioId)
                .Index(t => t.FaturamentoItemId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AutorizacaoProcedimento", "SolicitanteId", "dbo.SisMedico");
            DropForeignKey("dbo.AutorizacaoProcedimento", "PacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.AutorizacaoProcedimento", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AutorizacaoProcedimento", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.AutorizacaoProcedimento", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AutorizacaoProcedimento", new[] { "ConvenioId" });
            DropIndex("dbo.AutorizacaoProcedimento", new[] { "PacienteId" });
            DropIndex("dbo.AutorizacaoProcedimento", new[] { "SolicitanteId" });
            DropTable("dbo.AutorizacaoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AutorizacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
