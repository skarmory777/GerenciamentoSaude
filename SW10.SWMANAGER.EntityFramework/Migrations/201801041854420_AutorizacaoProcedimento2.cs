namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AutorizacaoProcedimento2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteAutorizacaoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SolicitanteId = c.Long(),
                    PacienteId = c.Long(),
                    ConvenioId = c.Long(nullable: false),
                    DataSolicitacao = c.DateTime(nullable: false),
                    Observacao = c.String(),
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
                .ForeignKey("dbo.SisPaciente", t => t.PacienteId)
                .ForeignKey("dbo.SisMedico", t => t.SolicitanteId)
                .Index(t => t.SolicitanteId)
                .Index(t => t.PacienteId)
                .Index(t => t.ConvenioId);

            CreateTable(
                "dbo.AteAutorizacaoProcedimentoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FaturamentoItemId = c.Long(nullable: false),
                    Senha = c.String(),
                    DataAutorizacao = c.DateTime(),
                    IsOrtese = c.Boolean(nullable: false),
                    AutorizadoPor = c.String(),
                    Quantidade = c.Int(nullable: false),
                    Status = c.Int(nullable: false),
                    AutorizacaoProcedimentoId = c.Long(nullable: false),
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
                    { "DynamicFilter_AutorizacaoProcedimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAutorizacaoProcedimento", t => t.AutorizacaoProcedimentoId, cascadeDelete: true)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: true)
                .Index(t => t.FaturamentoItemId)
                .Index(t => t.AutorizacaoProcedimentoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteAutorizacaoProcedimentoItem", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AteAutorizacaoProcedimentoItem", "AutorizacaoProcedimentoId", "dbo.AteAutorizacaoProcedimento");
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "SolicitanteId", "dbo.SisMedico");
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "PacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.AteAutorizacaoProcedimento", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.AteAutorizacaoProcedimentoItem", new[] { "AutorizacaoProcedimentoId" });
            DropIndex("dbo.AteAutorizacaoProcedimentoItem", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "ConvenioId" });
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "PacienteId" });
            DropIndex("dbo.AteAutorizacaoProcedimento", new[] { "SolicitanteId" });
            DropTable("dbo.AteAutorizacaoProcedimentoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AutorizacaoProcedimentoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteAutorizacaoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AutorizacaoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
