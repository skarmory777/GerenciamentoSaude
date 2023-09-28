namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class AjustesBalancoHidricoERegistroArquivos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PacienteAlergias",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataCadastro = c.DateTime(nullable: false),
                    Alergia = c.String(),
                    PacienteId = c.Long(nullable: false),
                    AtendimentoId = c.Long(),
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
                    { "DynamicFilter_PacienteAlergias_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId)
                .ForeignKey("dbo.SisPaciente", t => t.PacienteId, cascadeDelete: true)
                .Index(t => t.PacienteId)
                .Index(t => t.AtendimentoId);

            CreateTable(
                "dbo.PacienteDiagnosticos",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataDiagnostico = c.DateTime(nullable: false),
                    GrupoCIDId = c.Long(nullable: false),
                    PacienteId = c.Long(nullable: false),
                    AtendimentoId = c.Long(nullable: false),
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
                    { "DynamicFilter_PacienteDiagnosticos_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId, cascadeDelete: true)
                .ForeignKey("dbo.GrupoCID", t => t.GrupoCIDId, cascadeDelete: true)
                .ForeignKey("dbo.SisPaciente", t => t.PacienteId, cascadeDelete: true)
                .Index(t => t.GrupoCIDId)
                .Index(t => t.PacienteId)
                .Index(t => t.AtendimentoId);

            AddColumn("dbo.AteBalancoHidricoItens", "Dreno2", c => c.String());
            AddColumn("dbo.AteBalancoHidricoSinaisVitais", "Spo2", c => c.String());
            AddColumn("dbo.AteBalancoHidricoSinaisVitais", "Ins", c => c.String());
            AddColumn("dbo.SisRegistroArquivo", "ArquivoNome", c => c.String());
            AddColumn("dbo.SisRegistroArquivo", "ArquivoTipo", c => c.String());
        }

        public override void Down()
        {
            DropForeignKey("dbo.PacienteDiagnosticos", "PacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.PacienteDiagnosticos", "GrupoCIDId", "dbo.GrupoCID");
            DropForeignKey("dbo.PacienteDiagnosticos", "AtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.PacienteAlergias", "PacienteId", "dbo.SisPaciente");
            DropForeignKey("dbo.PacienteAlergias", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.PacienteDiagnosticos", new[] { "AtendimentoId" });
            DropIndex("dbo.PacienteDiagnosticos", new[] { "PacienteId" });
            DropIndex("dbo.PacienteDiagnosticos", new[] { "GrupoCIDId" });
            DropIndex("dbo.PacienteAlergias", new[] { "AtendimentoId" });
            DropIndex("dbo.PacienteAlergias", new[] { "PacienteId" });
            DropColumn("dbo.SisRegistroArquivo", "ArquivoTipo");
            DropColumn("dbo.SisRegistroArquivo", "ArquivoNome");
            DropColumn("dbo.AteBalancoHidricoSinaisVitais", "Ins");
            DropColumn("dbo.AteBalancoHidricoSinaisVitais", "Spo2");
            DropColumn("dbo.AteBalancoHidricoItens", "Dreno2");
            DropTable("dbo.PacienteDiagnosticos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PacienteDiagnosticos_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.PacienteAlergias",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PacienteAlergias_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
