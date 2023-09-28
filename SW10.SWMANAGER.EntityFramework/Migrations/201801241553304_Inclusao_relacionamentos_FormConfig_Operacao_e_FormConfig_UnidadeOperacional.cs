namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_relacionamentos_FormConfig_Operacao_e_FormConfig_UnidadeOperacional : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisFormConfigOperacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisFormConfigId = c.Long(),
                    SisOperacaoId = c.Long(),
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
                    { "DynamicFilter_FormConfigOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisFormConfig", t => t.SisFormConfigId)
                .ForeignKey("dbo.SisOperacao", t => t.SisOperacaoId)
                .Index(t => t.SisFormConfigId)
                .Index(t => t.SisOperacaoId);

            CreateTable(
                "dbo.SisFormConfigUnidadeOrganizacional",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisFormConfigId = c.Long(),
                    SisUnidadeOganizacionalId = c.Long(),
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
                    { "DynamicFilter_FormConfigUnidadeOrganizacional_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisFormConfig", t => t.SisFormConfigId)
                .ForeignKey("dbo.SisUnidadeOrganizacional", t => t.SisUnidadeOganizacionalId)
                .Index(t => t.SisFormConfigId)
                .Index(t => t.SisUnidadeOganizacionalId);

            AddColumn("dbo.SisOperacao", "Name", c => c.String(maxLength: 256));
            DropColumn("dbo.SisOperacao", "Label");
        }

        public override void Down()
        {
            AddColumn("dbo.SisOperacao", "Label", c => c.String());
            DropForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisUnidadeOganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisFormConfigId", "dbo.SisFormConfig");
            DropForeignKey("dbo.SisFormConfigOperacao", "SisOperacaoId", "dbo.SisOperacao");
            DropForeignKey("dbo.SisFormConfigOperacao", "SisFormConfigId", "dbo.SisFormConfig");
            DropIndex("dbo.SisFormConfigUnidadeOrganizacional", new[] { "SisUnidadeOganizacionalId" });
            DropIndex("dbo.SisFormConfigUnidadeOrganizacional", new[] { "SisFormConfigId" });
            DropIndex("dbo.SisFormConfigOperacao", new[] { "SisOperacaoId" });
            DropIndex("dbo.SisFormConfigOperacao", new[] { "SisFormConfigId" });
            DropColumn("dbo.SisOperacao", "Name");
            DropTable("dbo.SisFormConfigUnidadeOrganizacional",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfigUnidadeOrganizacional_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisFormConfigOperacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfigOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
