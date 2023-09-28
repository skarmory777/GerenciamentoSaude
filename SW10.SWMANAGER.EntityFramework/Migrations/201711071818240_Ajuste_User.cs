namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Ajuste_User : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AteAtendimentoLeitoMov", "AteAtendimentoId", "dbo.AteAtendimento");
            DropForeignKey("dbo.AteAtendimentoLeitoMov", "AteLeitoId", "dbo.AteLeito");
            DropForeignKey("dbo.AteAtendimentoLeitoMov", "UserId", "dbo.AbpUsers");
            DropForeignKey("dbo.SisFormConfigOperacao", "SisFormConfigId", "dbo.SisFormConfig");
            DropForeignKey("dbo.SisFormConfigOperacao", "SisOperacaoId", "dbo.SisOperacao");
            DropForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisFormConfigId", "dbo.SisFormConfig");
            DropForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisUnidadeOganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropIndex("dbo.AteAtendimentoLeitoMov", new[] { "UserId" });
            DropIndex("dbo.AteAtendimentoLeitoMov", new[] { "AteAtendimentoId" });
            DropIndex("dbo.AteAtendimentoLeitoMov", new[] { "AteLeitoId" });
            DropIndex("dbo.SisFormConfigOperacao", new[] { "SisFormConfigId" });
            DropIndex("dbo.SisFormConfigOperacao", new[] { "SisOperacaoId" });
            DropIndex("dbo.SisFormConfigUnidadeOrganizacional", new[] { "SisFormConfigId" });
            DropIndex("dbo.SisFormConfigUnidadeOrganizacional", new[] { "SisUnidadeOganizacionalId" });
            DropTable("dbo.AteAtendimentoLeitoMov",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_AtendimentoLeitoMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisFormConfigOperacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfigOperacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisFormConfigUnidadeOrganizacional",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormConfigUnidadeOrganizacional_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);

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
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.AteAtendimentoLeitoMov",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    DataInicial = c.DateTime(),
                    DataFinal = c.DateTime(),
                    DataInclusao = c.DateTime(),
                    UserId = c.Long(),
                    AteAtendimentoId = c.Long(),
                    AteLeitoId = c.Long(),
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
                    { "DynamicFilter_AtendimentoLeitoMov_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateIndex("dbo.SisFormConfigUnidadeOrganizacional", "SisUnidadeOganizacionalId");
            CreateIndex("dbo.SisFormConfigUnidadeOrganizacional", "SisFormConfigId");
            CreateIndex("dbo.SisFormConfigOperacao", "SisOperacaoId");
            CreateIndex("dbo.SisFormConfigOperacao", "SisFormConfigId");
            CreateIndex("dbo.AteAtendimentoLeitoMov", "AteLeitoId");
            CreateIndex("dbo.AteAtendimentoLeitoMov", "AteAtendimentoId");
            CreateIndex("dbo.AteAtendimentoLeitoMov", "UserId");
            AddForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisUnidadeOganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
            AddForeignKey("dbo.SisFormConfigUnidadeOrganizacional", "SisFormConfigId", "dbo.SisFormConfig", "Id");
            AddForeignKey("dbo.SisFormConfigOperacao", "SisOperacaoId", "dbo.SisOperacao", "Id");
            AddForeignKey("dbo.SisFormConfigOperacao", "SisFormConfigId", "dbo.SisFormConfig", "Id");
            AddForeignKey("dbo.AteAtendimentoLeitoMov", "UserId", "dbo.AbpUsers", "Id");
            AddForeignKey("dbo.AteAtendimentoLeitoMov", "AteLeitoId", "dbo.AteLeito", "Id");
            AddForeignKey("dbo.AteAtendimentoLeitoMov", "AteAtendimentoId", "dbo.AteAtendimento", "Id");
        }
    }
}
