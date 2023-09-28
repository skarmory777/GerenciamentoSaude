namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class TabelasFinanceiro : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FinFormaPagamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    NumeroParcelas = c.Int(nullable: false),
                    PercentualDesconto = c.Decimal(nullable: false, precision: 18, scale: 2),
                    DiasParcela1 = c.Int(),
                    DiasParcela2 = c.Int(),
                    DiasParcela3 = c.Int(),
                    DiasParcela4 = c.Int(),
                    DiasParcela5 = c.Int(),
                    DiasParcela6 = c.Int(),
                    DiasParcela7 = c.Int(),
                    DiasParcela8 = c.Int(),
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
                    { "DynamicFilter_FormaPagamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FinGrupoContaAdministrativa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsValorIncideResultadoOperacionalEmpresa = c.Boolean(nullable: false),
                    GrupoDREId = c.Long(nullable: false),
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
                    { "DynamicFilter_GrupoContaAdministrativa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinGrupoDRE", t => t.GrupoDREId, cascadeDelete: true)
                .Index(t => t.GrupoDREId);

            CreateTable(
                "dbo.FinGrupoDRE",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_GrupoDRE_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FinSituacaoLancamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsPermiteAlteracao = c.Boolean(nullable: false),
                    CorLancamento = c.String(),
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
                    { "DynamicFilter_SituacaoLancamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FinSubGrupoContaAdministrativa",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    GrupoContaAdministrativaId = c.Long(nullable: false),
                    IsSubGrupoContaNaoOperacional = c.Boolean(nullable: false),
                    IsUtilizadoCalculoSalario = c.Boolean(nullable: false),
                    IsSomandoDespesas = c.Boolean(nullable: false),
                    IsUsarFormula = c.Boolean(nullable: false),
                    IsNaoDetalharContaAdministrativa = c.Boolean(nullable: false),
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
                    { "DynamicFilter_SubGrupoContaAdministrativa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinGrupoContaAdministrativa", t => t.GrupoContaAdministrativaId, cascadeDelete: true)
                .Index(t => t.GrupoContaAdministrativaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", "dbo.FinGrupoContaAdministrativa");
            DropForeignKey("dbo.FinGrupoContaAdministrativa", "GrupoDREId", "dbo.FinGrupoDRE");
            DropIndex("dbo.FinSubGrupoContaAdministrativa", new[] { "GrupoContaAdministrativaId" });
            DropIndex("dbo.FinGrupoContaAdministrativa", new[] { "GrupoDREId" });
            DropTable("dbo.FinSubGrupoContaAdministrativa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubGrupoContaAdministrativa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinSituacaoLancamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SituacaoLancamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinGrupoDRE",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoDRE_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinGrupoContaAdministrativa",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_GrupoContaAdministrativa_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FinFormaPagamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormaPagamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
