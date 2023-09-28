namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Inclusao_Tabela_SisBI : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisBI",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisModuloId = c.Long(),
                    SisOperacaoId = c.Long(),
                    Url = c.String(),
                    IsPublico = c.Boolean(nullable: false),
                    IsPrincipal = c.Boolean(nullable: false),
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
                    { "DynamicFilter_BI_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisModulo", t => t.SisModuloId)
                .ForeignKey("dbo.SisOperacao", t => t.SisOperacaoId)
                .Index(t => t.SisModuloId)
                .Index(t => t.SisOperacaoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisBI", "SisOperacaoId", "dbo.SisOperacao");
            DropForeignKey("dbo.SisBI", "SisModuloId", "dbo.SisModulo");
            DropIndex("dbo.SisBI", new[] { "SisOperacaoId" });
            DropIndex("dbo.SisBI", new[] { "SisModuloId" });
            DropTable("dbo.SisBI",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_BI_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
