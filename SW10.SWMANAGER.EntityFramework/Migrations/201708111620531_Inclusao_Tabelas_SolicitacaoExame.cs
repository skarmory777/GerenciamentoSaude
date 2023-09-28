namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_Tabelas_SolicitacaoExame : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabKitExame",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 50),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_KitExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssPrescricaoMedica",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    DataPrescricao = c.DateTime(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_PrescricaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssSolicitacaoExame",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    AtendimentoId = c.Long(),
                    DataSolicitacao = c.DateTime(nullable: false),
                    OrigemId = c.Long(),
                    LeitoId = c.Long(),
                    Prioridade = c.Int(nullable: false),
                    UnidadeOrganizacionalId = c.Long(),
                    MedicoSolicitanteId = c.Long(),
                    Observacao = c.String(),
                    PrescricaoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_SolicitacaoExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Atendimento", t => t.AtendimentoId)
                .ForeignKey("dbo.Leito", t => t.LeitoId)
                .ForeignKey("dbo.Medico", t => t.MedicoSolicitanteId)
                .ForeignKey("dbo.AssPrescricaoMedica", t => t.PrescricaoId)
                .ForeignKey("dbo.UnidadeOrganizacional", t => t.UnidadeOrganizacionalId)
                .Index(t => t.AtendimentoId)
                .Index(t => t.LeitoId)
                .Index(t => t.UnidadeOrganizacionalId)
                .Index(t => t.MedicoSolicitanteId)
                .Index(t => t.PrescricaoId);

            CreateTable(
                "dbo.AssSolicitacaoExameItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SolicitacaoExameId = c.Long(nullable: false),
                    FaturamentoItemId = c.Long(),
                    GuiaNumero = c.String(),
                    DataValidade = c.DateTime(),
                    SenhaNumero = c.String(),
                    MaterialId = c.Long(),
                    Justificativa = c.String(),
                    KitExameId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
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
                    { "DynamicFilter_SolicitacaoExameItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId)
                .ForeignKey("dbo.LabKitExame", t => t.KitExameId)
                .ForeignKey("dbo.Material", t => t.MaterialId)
                .ForeignKey("dbo.AssSolicitacaoExame", t => t.SolicitacaoExameId, cascadeDelete: false)
                .Index(t => t.SolicitacaoExameId)
                .Index(t => t.FaturamentoItemId)
                .Index(t => t.MaterialId)
                .Index(t => t.KitExameId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AssSolicitacaoExame", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropForeignKey("dbo.AssSolicitacaoExameItem", "SolicitacaoExameId", "dbo.AssSolicitacaoExame");
            DropForeignKey("dbo.AssSolicitacaoExameItem", "MaterialId", "dbo.Material");
            DropForeignKey("dbo.AssSolicitacaoExameItem", "KitExameId", "dbo.LabKitExame");
            DropForeignKey("dbo.AssSolicitacaoExameItem", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.AssSolicitacaoExame", "PrescricaoId", "dbo.AssPrescricaoMedica");
            DropForeignKey("dbo.AssSolicitacaoExame", "MedicoSolicitanteId", "dbo.Medico");
            DropForeignKey("dbo.AssSolicitacaoExame", "LeitoId", "dbo.Leito");
            DropForeignKey("dbo.AssSolicitacaoExame", "AtendimentoId", "dbo.Atendimento");
            DropIndex("dbo.AssSolicitacaoExameItem", new[] { "KitExameId" });
            DropIndex("dbo.AssSolicitacaoExameItem", new[] { "MaterialId" });
            DropIndex("dbo.AssSolicitacaoExameItem", new[] { "FaturamentoItemId" });
            DropIndex("dbo.AssSolicitacaoExameItem", new[] { "SolicitacaoExameId" });
            DropIndex("dbo.AssSolicitacaoExame", new[] { "PrescricaoId" });
            DropIndex("dbo.AssSolicitacaoExame", new[] { "MedicoSolicitanteId" });
            DropIndex("dbo.AssSolicitacaoExame", new[] { "UnidadeOrganizacionalId" });
            DropIndex("dbo.AssSolicitacaoExame", new[] { "LeitoId" });
            DropIndex("dbo.AssSolicitacaoExame", new[] { "AtendimentoId" });
            DropTable("dbo.AssSolicitacaoExameItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoExameItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssSolicitacaoExame",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SolicitacaoExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssPrescricaoMedica",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrescricaoMedica_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.LabKitExame",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_KitExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
