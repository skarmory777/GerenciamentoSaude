namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class FatAutorizacao_ItemRelacionamento : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatAutorizacaoDetalhe",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatAutorizacaoId = c.Long(),
                    SisConvenioId = c.Long(),
                    SisPlanoId = c.Long(),
                    FatGrupoId = c.Long(),
                    FatSubGrupoId = c.Long(),
                    FatItemId = c.Long(),
                    SisUnidadeOrganizacionalId = c.Long(),
                    IsLimiteQtd = c.Boolean(nullable: false),
                    QtdMinimo = c.Int(nullable: false),
                    QtdMaximo = c.Int(nullable: false),
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
                    { "DynamicFilter_FaturamentoAutorizacaoDetalhe_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatAutorizacao", t => t.FatAutorizacaoId)
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId)
                .ForeignKey("dbo.FatGrupo", t => t.FatGrupoId)
                .ForeignKey("dbo.FatItem", t => t.FatItemId)
                .ForeignKey("dbo.SisPlano", t => t.SisPlanoId)
                .ForeignKey("dbo.FatSubGrupo", t => t.FatSubGrupoId)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.SisUnidadeOrganizacionalId)
                .Index(t => t.FatAutorizacaoId)
                .Index(t => t.SisConvenioId)
                .Index(t => t.SisPlanoId)
                .Index(t => t.FatGrupoId)
                .Index(t => t.FatSubGrupoId)
                .Index(t => t.FatItemId)
                .Index(t => t.SisUnidadeOrganizacionalId);

            CreateTable(
                "dbo.FatAutorizacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Mensagem = c.String(),
                    DataInicial = c.DateTime(nullable: false),
                    DataFinal = c.DateTime(),
                    IsAmbulatorio = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsAutorizacao = c.Boolean(nullable: false),
                    IsLiberacao = c.Boolean(nullable: false),
                    IsJustificativa = c.Boolean(nullable: false),
                    IsBloqueio = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoAutorizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FatConvenioRelacionamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisConvenioId = c.Long(),
                    FatGrupoId = c.Long(),
                    FatSubGrupoId = c.Long(),
                    FatTabelaRelacionamentoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoConvenioRelacionamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId)
                .ForeignKey("dbo.FatGrupo", t => t.FatGrupoId)
                .ForeignKey("dbo.FatSubGrupo", t => t.FatSubGrupoId)
                .ForeignKey("dbo.FatTabelaRelacionamento", t => t.FatTabelaRelacionamentoId)
                .Index(t => t.SisConvenioId)
                .Index(t => t.FatGrupoId)
                .Index(t => t.FatSubGrupoId)
                .Index(t => t.FatTabelaRelacionamentoId);

            CreateTable(
                "dbo.FatTabelaRelacionamento",
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
                    { "DynamicFilter_FaturamentoTabelaRelacionamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FatItemRelacionamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    FatItemOrigemId = c.Long(),
                    SisConvenioId = c.Long(),
                    FatTabelaRelacionamentoId = c.Long(),
                    FatItemDestinoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoItemRelacionamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId)
                .ForeignKey("dbo.FatItem", t => t.FatItemDestinoId)
                .ForeignKey("dbo.FatItem", t => t.FatItemOrigemId)
                .ForeignKey("dbo.FatTabelaRelacionamento", t => t.FatTabelaRelacionamentoId)
                .Index(t => t.FatItemOrigemId)
                .Index(t => t.SisConvenioId)
                .Index(t => t.FatTabelaRelacionamentoId)
                .Index(t => t.FatItemDestinoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItemRelacionamento", "FatTabelaRelacionamentoId", "dbo.FatTabelaRelacionamento");
            DropForeignKey("dbo.FatItemRelacionamento", "FatItemOrigemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemRelacionamento", "FatItemDestinoId", "dbo.FatItem");
            DropForeignKey("dbo.FatItemRelacionamento", "SisConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.FatConvenioRelacionamento", "FatTabelaRelacionamentoId", "dbo.FatTabelaRelacionamento");
            DropForeignKey("dbo.FatConvenioRelacionamento", "FatSubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.FatConvenioRelacionamento", "FatGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatConvenioRelacionamento", "SisConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "SisUnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "FatSubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "SisPlanoId", "dbo.SisPlano");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "FatGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "SisConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.FatAutorizacaoDetalhe", "FatAutorizacaoId", "dbo.FatAutorizacao");
            DropIndex("dbo.FatItemRelacionamento", new[] { "FatItemDestinoId" });
            DropIndex("dbo.FatItemRelacionamento", new[] { "FatTabelaRelacionamentoId" });
            DropIndex("dbo.FatItemRelacionamento", new[] { "SisConvenioId" });
            DropIndex("dbo.FatItemRelacionamento", new[] { "FatItemOrigemId" });
            DropIndex("dbo.FatConvenioRelacionamento", new[] { "FatTabelaRelacionamentoId" });
            DropIndex("dbo.FatConvenioRelacionamento", new[] { "FatSubGrupoId" });
            DropIndex("dbo.FatConvenioRelacionamento", new[] { "FatGrupoId" });
            DropIndex("dbo.FatConvenioRelacionamento", new[] { "SisConvenioId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "SisUnidadeOrganizacionalId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "FatItemId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "FatSubGrupoId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "FatGrupoId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "SisPlanoId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "SisConvenioId" });
            DropIndex("dbo.FatAutorizacaoDetalhe", new[] { "FatAutorizacaoId" });
            DropTable("dbo.FatItemRelacionamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoItemRelacionamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTabelaRelacionamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoTabelaRelacionamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatConvenioRelacionamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoConvenioRelacionamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatAutorizacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoAutorizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatAutorizacaoDetalhe",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoAutorizacaoDetalhe_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
