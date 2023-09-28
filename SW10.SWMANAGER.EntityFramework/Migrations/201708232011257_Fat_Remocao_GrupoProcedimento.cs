namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_Remocao_GrupoProcedimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FatSubGrupoProcedimento", "GrupoProcedimentoId", "dbo.FatGrupoProcedimento");
            DropForeignKey("dbo.FatGrupoProcedimento", "GrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatGrupoProcedimento", "TipoGrupoId", "dbo.FatTipoGrupo");
            DropForeignKey("dbo.FatItem", "GrupoProcedimentoId", "dbo.FatGrupoProcedimento");
            DropIndex("dbo.FatSubGrupoProcedimento", new[] { "GrupoProcedimentoId" });
            DropIndex("dbo.FatGrupoProcedimento", new[] { "TipoGrupoId" });
            DropIndex("dbo.FatGrupoProcedimento", new[] { "GrupoId" });
            DropIndex("dbo.FatItem", new[] { "GrupoProcedimentoId" });
            DropColumn("dbo.FatItem", "GrupoProcedimentoId");
            DropTable("dbo.FatSubGrupoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SubGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatGrupoProcedimento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoGrupoProcedimento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
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

            AddColumn("dbo.FatItem", "GrupoProcedimentoId", c => c.Long(nullable: false));
            CreateIndex("dbo.FatItem", "GrupoProcedimentoId");
            CreateIndex("dbo.FatGrupoProcedimento", "GrupoId");
            CreateIndex("dbo.FatGrupoProcedimento", "TipoGrupoId");
            AddForeignKey("dbo.FatItem", "GrupoProcedimentoId", "dbo.FatGrupoProcedimento", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FatGrupoProcedimento", "TipoGrupoId", "dbo.FatTipoGrupo", "Id", cascadeDelete: true);
            AddForeignKey("dbo.FatGrupoProcedimento", "GrupoId", "dbo.FatGrupo", "Id", cascadeDelete: true);
        }
    }
}
