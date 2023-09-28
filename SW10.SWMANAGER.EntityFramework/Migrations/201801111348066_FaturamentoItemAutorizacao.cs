namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class FaturamentoItemAutorizacao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatFaturamentoItemAutorizacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    ConvenioId = c.Long(nullable: false),
                    FaturamentoItemId = c.Long(nullable: false),
                    FaturamentoGrupoId = c.Long(),
                    FaturamentoSubGrupoId = c.Long(),
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
                    { "DynamicFilter_FaturamentoItemAutorizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId, cascadeDelete: true)
                .ForeignKey("dbo.FatGrupo", t => t.FaturamentoGrupoId)
                .ForeignKey("dbo.FatItem", t => t.FaturamentoItemId, cascadeDelete: true)
                .ForeignKey("dbo.FatSubGrupo", t => t.FaturamentoSubGrupoId)
                .Index(t => t.ConvenioId)
                .Index(t => t.FaturamentoItemId)
                .Index(t => t.FaturamentoGrupoId)
                .Index(t => t.FaturamentoSubGrupoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoSubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatFaturamentoItemAutorizacao", "FaturamentoGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatFaturamentoItemAutorizacao", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatFaturamentoItemAutorizacao", new[] { "FaturamentoSubGrupoId" });
            DropIndex("dbo.FatFaturamentoItemAutorizacao", new[] { "FaturamentoGrupoId" });
            DropIndex("dbo.FatFaturamentoItemAutorizacao", new[] { "FaturamentoItemId" });
            DropIndex("dbo.FatFaturamentoItemAutorizacao", new[] { "ConvenioId" });
            DropTable("dbo.FatFaturamentoItemAutorizacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoItemAutorizacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
