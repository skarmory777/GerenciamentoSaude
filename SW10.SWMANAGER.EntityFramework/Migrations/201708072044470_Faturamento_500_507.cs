namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Faturamento_500_507 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
                    TipoGrupoId = c.Long(),
                    IsAtivo = c.Boolean(nullable: false),
                    IsPacote = c.Boolean(nullable: false),
                    IsOpme = c.Boolean(nullable: false),
                    IsLaboratorio = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatTipoGrupo", t => t.TipoGrupoId)
                .Index(t => t.TipoGrupoId);

            CreateTable(
                "dbo.FatTipoGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
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
                    { "DynamicFilter_TipoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FatGrupoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
                    TipoGrupoId = c.Long(nullable: false),
                    GrupoId = c.Long(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
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
                    { "DynamicFilter_FaturamentoGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGrupo", t => t.GrupoId, cascadeDelete: false)
                .ForeignKey("dbo.FatTipoGrupo", t => t.TipoGrupoId, cascadeDelete: false)
                .Index(t => t.TipoGrupoId)
                .Index(t => t.GrupoId);

            CreateTable(
                "dbo.FatItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 100),
                    GrupoId = c.Long(nullable: false),
                    GrupoProcedimentoId = c.Long(nullable: false),
                    SubGrupoId = c.Long(),
                    DescricaoTuss = c.String(maxLength: 255),
                    Observacao = c.String(maxLength: 255),
                    CodAmb = c.String(maxLength: 20),
                    CodTuss = c.String(maxLength: 20),
                    CodCbhpm = c.String(maxLength: 20),
                    Sexo = c.Int(nullable: false),
                    QtdLaudo = c.Int(nullable: false),
                    TipoLaudo = c.Int(nullable: false),
                    DuracaoMinima = c.Int(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
                    IsObrigaMedico = c.Boolean(nullable: false),
                    IsTaxaUrgencia = c.Boolean(nullable: false),
                    IsPediatria = c.Boolean(nullable: false),
                    IsProcedimentoSerie = c.Boolean(nullable: false),
                    IsRequisicaoExame = c.Boolean(nullable: false),
                    IsPermiteRevisao = c.Boolean(nullable: false),
                    IsPrecoManual = c.Boolean(nullable: false),
                    IsAutorizacao = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsAmbulatorio = c.Boolean(nullable: false),
                    IsCirurgia = c.Boolean(nullable: false),
                    IsPorte = c.Boolean(nullable: false),
                    IsConsultor = c.Boolean(nullable: false),
                    IsLaboratorio = c.Boolean(nullable: false),
                    IsPlantonista = c.Boolean(nullable: false),
                    IsOpme = c.Boolean(nullable: false),
                    IsExtraCaixa = c.Boolean(nullable: false),
                    IsLaudo = c.Boolean(nullable: false),
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
                    { "DynamicFilter_Item_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGrupo", t => t.GrupoId, cascadeDelete: false)
                .ForeignKey("dbo.FatGrupoProcedimento", t => t.GrupoProcedimentoId, cascadeDelete: false)
                .ForeignKey("dbo.FatSubGrupo", t => t.SubGrupoId)
                .Index(t => t.GrupoId)
                .Index(t => t.GrupoProcedimentoId)
                .Index(t => t.SubGrupoId);

            CreateTable(
                "dbo.FatSubGrupo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
                    GrupoId = c.Long(nullable: false),
                    IsLaboratorio = c.Boolean(nullable: false),
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
                    { "DynamicFilter_SubGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGrupo", t => t.GrupoId, cascadeDelete: false)
                .Index(t => t.GrupoId);

            CreateTable(
                "dbo.FatItemTabela",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TabelaId = c.Long(nullable: false),
                    ItemId = c.Long(nullable: false),
                    VigenciaDataInicio = c.DateTime(nullable: false),
                    ValorHonorario = c.Single(nullable: false),
                    ValorOperacional = c.Single(nullable: false),
                    ValorTotal = c.Single(nullable: false),
                    IsAtivo = c.Boolean(nullable: false),
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
                    { "DynamicFilter_ItemTabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.ItemId, cascadeDelete: false)
                .ForeignKey("dbo.FatTabela", t => t.TabelaId, cascadeDelete: false)
                .Index(t => t.TabelaId)
                .Index(t => t.ItemId);

            CreateTable(
                "dbo.FatTabela",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
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
                    { "DynamicFilter_Tabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.FatSubGrupoProcedimento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 100),
                    GrupoProcedimentoId = c.Long(nullable: false),
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
                    { "DynamicFilter_SubGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatGrupoProcedimento", t => t.GrupoProcedimentoId, cascadeDelete: false)
                .Index(t => t.GrupoProcedimentoId);

            CreateTable(
                "dbo.FatTabelaPrecoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    TabelaPrecoId = c.Long(nullable: false),
                    ItemId = c.Long(nullable: false),
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
                    { "DynamicFilter_TabelaPrecoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.ItemId, cascadeDelete: false)
                .ForeignKey("dbo.FatTabelaPreco", t => t.TabelaPrecoId, cascadeDelete: false)
                .Index(t => t.TabelaPrecoId)
                .Index(t => t.ItemId);

            CreateTable(
                "dbo.FatTabelaPreco",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(maxLength: 100),
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
                    { "DynamicFilter_TabelaPreco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTabelaPrecoItem", "TabelaPrecoId", "dbo.FatTabelaPreco");
            DropForeignKey("dbo.FatTabelaPrecoItem", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatSubGrupoProcedimento", "GrupoProcedimentoId", "dbo.FatGrupoProcedimento");
            DropForeignKey("dbo.FatItemTabela", "TabelaId", "dbo.FatTabela");
            DropForeignKey("dbo.FatItemTabela", "ItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatItem", "SubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.FatSubGrupo", "GrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatItem", "GrupoProcedimentoId", "dbo.FatGrupoProcedimento");
            DropForeignKey("dbo.FatItem", "GrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatGrupoProcedimento", "TipoGrupoId", "dbo.FatTipoGrupo");
            DropForeignKey("dbo.FatGrupoProcedimento", "GrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatGrupo", "TipoGrupoId", "dbo.FatTipoGrupo");
            DropIndex("dbo.FatTabelaPrecoItem", new[] { "ItemId" });
            DropIndex("dbo.FatTabelaPrecoItem", new[] { "TabelaPrecoId" });
            DropIndex("dbo.FatSubGrupoProcedimento", new[] { "GrupoProcedimentoId" });
            DropIndex("dbo.FatItemTabela", new[] { "ItemId" });
            DropIndex("dbo.FatItemTabela", new[] { "TabelaId" });
            DropIndex("dbo.FatSubGrupo", new[] { "GrupoId" });
            DropIndex("dbo.FatItem", new[] { "SubGrupoId" });
            DropIndex("dbo.FatItem", new[] { "GrupoProcedimentoId" });
            DropIndex("dbo.FatItem", new[] { "GrupoId" });
            DropIndex("dbo.FatGrupoProcedimento", new[] { "GrupoId" });
            DropIndex("dbo.FatGrupoProcedimento", new[] { "TipoGrupoId" });
            DropIndex("dbo.FatGrupo", new[] { "TipoGrupoId" });
            DropTable("dbo.FatTabelaPreco",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TabelaPreco_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTabelaPrecoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TabelaPrecoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatSubGrupoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTabela",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Tabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatItemTabela",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ItemTabela_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatSubGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Item_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatGrupoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatTipoGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatGrupo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGrupo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
