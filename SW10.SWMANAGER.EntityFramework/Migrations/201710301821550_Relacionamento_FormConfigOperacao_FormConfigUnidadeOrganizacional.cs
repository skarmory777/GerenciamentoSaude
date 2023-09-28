namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Relacionamento_FormConfigOperacao_FormConfigUnidadeOrganizacional : DbMigration
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

            AddColumn("dbo.SisOperacao", "IsFormulario", c => c.Boolean(nullable: false));
            AddColumn("dbo.AssProntuario", "SisOperacaoId", c => c.Long());
            AddColumn("dbo.AssProntuario", "AssProntuarioPrincipalId", c => c.Long());
            CreateIndex("dbo.AssProntuario", "SisOperacaoId");
            CreateIndex("dbo.AssProntuario", "AssProntuarioPrincipalId");
            AddForeignKey("dbo.AssProntuario", "SisOperacaoId", "dbo.SisOperacao", "Id");
            AddForeignKey("dbo.AssProntuario", "AssProntuarioPrincipalId", "dbo.AssProntuario", "Id");
            DropColumn("dbo.AssProntuario", "OperacaoModuloId");
        }

        public override void Down()
        {
            AddColumn("dbo.AssProntuario", "OperacaoModuloId", c => c.Long());
            DropForeignKey("dbo.AssProntuario", "AssProntuarioPrincipalId", "dbo.AssProntuario");
            DropForeignKey("dbo.AssProntuario", "SisOperacaoId", "dbo.SisOperacao");
            DropForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisUnidadeOganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisFormConfigId", "dbo.SisFormConfig");
            DropForeignKey("dbo.SisFormConfigOperacao", "SisOperacaoId", "dbo.SisOperacao");
            DropForeignKey("dbo.SisFormConfigOperacao", "SisFormConfigId", "dbo.SisFormConfig");
            DropIndex("dbo.AssProntuario", new[] { "AssProntuarioPrincipalId" });
            DropIndex("dbo.AssProntuario", new[] { "SisOperacaoId" });
            DropIndex("dbo.SisFormConfigUnidadeOrganizacional", new[] { "SisUnidadeOganizacionalId" });
            DropIndex("dbo.SisFormConfigUnidadeOrganizacional", new[] { "SisFormConfigId" });
            DropIndex("dbo.SisFormConfigOperacao", new[] { "SisOperacaoId" });
            DropIndex("dbo.SisFormConfigOperacao", new[] { "SisFormConfigId" });
            DropColumn("dbo.AssProntuario", "AssProntuarioPrincipalId");
            DropColumn("dbo.AssProntuario", "SisOperacaoId");
            DropColumn("dbo.SisOperacao", "IsFormulario");
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
