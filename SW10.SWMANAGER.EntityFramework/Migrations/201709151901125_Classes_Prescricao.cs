namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Classes_Prescricao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssDivisao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 60),
                    IsMedico = c.Boolean(nullable: false),
                    IsEnfermagem = c.Boolean(nullable: false),
                    Ordem = c.Int(nullable: false),
                    AssDivisaoId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_Divisao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssDivisao", t => t.AssDivisaoId)
                .Index(t => t.AssDivisaoId);

            CreateTable(
                "dbo.SisElementoHtml",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsRequerido = c.Boolean(nullable: false),
                    Rotulo = c.String(),
                    RotuloPosElemento = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_ElementoHtml_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssFrequencia",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_Frequencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssTipoRespostaConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_TipoRespostaConfiguracao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssTipoRespostaConfigElementoHtml",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisElementoHtmlId = c.Long(nullable: false),
                    AssTipoRespostaConfiguracaoId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_TipoRespostaConfiguracaoElementoHtml_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisElementoHtml", t => t.SisElementoHtmlId, cascadeDelete: false)
                .ForeignKey("dbo.AssTipoRespostaConfig", t => t.AssTipoRespostaConfiguracaoId, cascadeDelete: false)
                .Index(t => t.SisElementoHtmlId)
                .Index(t => t.AssTipoRespostaConfiguracaoId);

            CreateTable(
                "dbo.AssTipoResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(maxLength: 60),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_TipoResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AssTipoRespostaAssTipoRespostaConfig",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AssTipoRespostaId = c.Long(nullable: false),
                    AssTipoRespostaConfigId = c.Long(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
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
                    { "DynamicFilter_TipoRespostaTipoRespostaConfiguracao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssTipoResposta", t => t.AssTipoRespostaId, cascadeDelete: false)
                .ForeignKey("dbo.AssTipoRespostaConfig", t => t.AssTipoRespostaConfigId, cascadeDelete: false)
                .Index(t => t.AssTipoRespostaId)
                .Index(t => t.AssTipoRespostaConfigId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AssTipoRespostaAssTipoRespostaConfig", "AssTipoRespostaConfigId", "dbo.AssTipoRespostaConfig");
            DropForeignKey("dbo.AssTipoRespostaAssTipoRespostaConfig", "AssTipoRespostaId", "dbo.AssTipoResposta");
            DropForeignKey("dbo.AssTipoRespostaConfigElementoHtml", "AssTipoRespostaConfiguracaoId", "dbo.AssTipoRespostaConfig");
            DropForeignKey("dbo.AssTipoRespostaConfigElementoHtml", "SisElementoHtmlId", "dbo.SisElementoHtml");
            DropForeignKey("dbo.AssDivisao", "AssDivisaoId", "dbo.AssDivisao");
            DropIndex("dbo.AssTipoRespostaAssTipoRespostaConfig", new[] { "AssTipoRespostaConfigId" });
            DropIndex("dbo.AssTipoRespostaAssTipoRespostaConfig", new[] { "AssTipoRespostaId" });
            DropIndex("dbo.AssTipoRespostaConfigElementoHtml", new[] { "AssTipoRespostaConfiguracaoId" });
            DropIndex("dbo.AssTipoRespostaConfigElementoHtml", new[] { "SisElementoHtmlId" });
            DropIndex("dbo.AssDivisao", new[] { "AssDivisaoId" });
            DropTable("dbo.AssTipoRespostaAssTipoRespostaConfig",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoRespostaTipoRespostaConfiguracao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssTipoResposta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssTipoRespostaConfigElementoHtml",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoRespostaConfiguracaoElementoHtml_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssTipoRespostaConfig",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoRespostaConfiguracao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssFrequencia",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Frequencia_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisElementoHtml",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_ElementoHtml_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssDivisao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_Divisao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
