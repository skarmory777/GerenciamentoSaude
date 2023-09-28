namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class CriacaoBalancoHidrico : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AteBalancoHidricos",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AtendimentoId = c.Long(nullable: false),
                    DataBalancoHidrico = c.DateTime(nullable: false),
                    HoraIntervalo = c.Int(nullable: false),
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
                    { "DynamicFilter_BalancoHidrico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteAtendimento", t => t.AtendimentoId, cascadeDelete: true)
                .Index(t => t.AtendimentoId);

            CreateTable(
                "dbo.AteBalancoHidricoItens",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    BalancoHidricoId = c.Long(nullable: false),
                    Hora = c.Time(nullable: false, precision: 7),
                    SinaisVitaisId = c.Long(nullable: false),
                    SangueDerivados = c.String(),
                    IngestVoSne = c.String(),
                    Diurese = c.String(),
                    Dreno = c.String(),
                    TotalParcial = c.Boolean(nullable: false),
                    TotalGeral = c.Boolean(nullable: false),
                    ResponsavelAssinatura = c.Long(nullable: false),
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
                    { "DynamicFilter_BalancoHidricoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteBalancoHidricos", t => t.BalancoHidricoId, cascadeDelete: true)
                .ForeignKey("dbo.AteBalancoHidricoSinaisVitais", t => t.SinaisVitaisId, cascadeDelete: true)
                .Index(t => t.BalancoHidricoId)
                .Index(t => t.SinaisVitaisId);

            CreateTable(
                "dbo.AteBalancoHidricoEndovenosos",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    BalancoHidricoItemId = c.Long(nullable: false),
                    IndiceSolucao = c.Int(nullable: false),
                    Valor = c.String(),
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
                    { "DynamicFilter_BalancoHidricoEndovenoso_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteBalancoHidricoItens", t => t.BalancoHidricoItemId, cascadeDelete: true)
                .Index(t => t.BalancoHidricoItemId);

            CreateTable(
                "dbo.AteBalancoHidricoSinaisVitais",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Temperatura = c.String(),
                    Pulso = c.String(),
                    Respiracao = c.String(),
                    PressaoSistolica = c.String(),
                    PressaoDiastolica = c.String(),
                    PressaoVenosaCentral = c.String(),
                    EscalaDeDor = c.String(),
                    Hemoglucoteste = c.String(),
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
                    { "DynamicFilter_BalancoHidricoSinaisVitais_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteBalancoHidricoSolucoes",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    BalancoHidricoId = c.Long(nullable: false),
                    IndiceSolucao = c.Int(nullable: false),
                    Valor = c.String(),
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
                    { "DynamicFilter_BalancoHidricoSolucoes_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AteBalancoHidricos", t => t.BalancoHidricoId, cascadeDelete: true)
                .Index(t => t.BalancoHidricoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AteBalancoHidricoSolucoes", "BalancoHidricoId", "dbo.AteBalancoHidricos");
            DropForeignKey("dbo.AteBalancoHidricoItens", "SinaisVitaisId", "dbo.AteBalancoHidricoSinaisVitais");
            DropForeignKey("dbo.AteBalancoHidricoEndovenosos", "BalancoHidricoItemId", "dbo.AteBalancoHidricoItens");
            DropForeignKey("dbo.AteBalancoHidricoItens", "BalancoHidricoId", "dbo.AteBalancoHidricos");
            DropForeignKey("dbo.AteBalancoHidricos", "AtendimentoId", "dbo.AteAtendimento");
            DropIndex("dbo.AteBalancoHidricoSolucoes", new[] { "BalancoHidricoId" });
            DropIndex("dbo.AteBalancoHidricoEndovenosos", new[] { "BalancoHidricoItemId" });
            DropIndex("dbo.AteBalancoHidricoItens", new[] { "SinaisVitaisId" });
            DropIndex("dbo.AteBalancoHidricoItens", new[] { "BalancoHidricoId" });
            DropIndex("dbo.AteBalancoHidricos", new[] { "AtendimentoId" });
            DropTable("dbo.AteBalancoHidricoSolucoes",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BalancoHidricoSolucoes_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteBalancoHidricoSinaisVitais",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BalancoHidricoSinaisVitais_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteBalancoHidricoEndovenosos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BalancoHidricoEndovenoso_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteBalancoHidricoItens",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BalancoHidricoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AteBalancoHidricos",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BalancoHidrico_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
