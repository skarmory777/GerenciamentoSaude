namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Divisao_Adicionando_Relacionamento_TipoResposta : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssDivisaoTipoResposta",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    AssDivisaoId = c.Long(nullable: false),
                    AssTipoRespostaId = c.Long(nullable: false),
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
                    { "DynamicFilter_DivisaoTipoResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssDivisao", t => t.AssDivisaoId, cascadeDelete: false)
                .ForeignKey("dbo.AssTipoResposta", t => t.AssTipoRespostaId, cascadeDelete: false)
                .Index(t => t.AssDivisaoId)
                .Index(t => t.AssTipoRespostaId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AssDivisaoTipoResposta", "AssTipoRespostaId", "dbo.AssTipoResposta");
            DropForeignKey("dbo.AssDivisaoTipoResposta", "AssDivisaoId", "dbo.AssDivisao");
            DropIndex("dbo.AssDivisaoTipoResposta", new[] { "AssTipoRespostaId" });
            DropIndex("dbo.AssDivisaoTipoResposta", new[] { "AssDivisaoId" });
            DropTable("dbo.AssDivisaoTipoResposta",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_DivisaoTipoResposta_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
