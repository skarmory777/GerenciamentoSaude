namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class FatConfigConvenioGlobal : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatConfigConvenioGlobal",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EmpresaId = c.Long(),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
                    FatGrupoId = c.Long(),
                    FatSubGrupoId = c.Long(),
                    FatTabelaGlobalId = c.Long(),
                    FatItemId = c.Long(),
                    DataIncio = c.DateTime(),
                    DataFim = c.DateTime(),
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
                    { "DynamicFilter_FaturamentoConfigConvenioGlobal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId)
                .ForeignKey("dbo.FatGrupo", t => t.FatGrupoId)
                .ForeignKey("dbo.FatItem", t => t.FatItemId)
                .ForeignKey("dbo.SisPlano", t => t.PlanoId)
                .ForeignKey("dbo.FatSubGrupo", t => t.FatSubGrupoId)
                .ForeignKey("dbo.FatGlobal", t => t.FatTabelaGlobalId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId)
                .Index(t => t.FatGrupoId)
                .Index(t => t.FatSubGrupoId)
                .Index(t => t.FatTabelaGlobalId)
                .Index(t => t.FatItemId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatConfigConvenioGlobal", "FatTabelaGlobalId", "dbo.FatGlobal");
            DropForeignKey("dbo.FatConfigConvenioGlobal", "FatSubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.FatConfigConvenioGlobal", "PlanoId", "dbo.SisPlano");
            DropForeignKey("dbo.FatConfigConvenioGlobal", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatConfigConvenioGlobal", "FatGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatConfigConvenioGlobal", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FatConfigConvenioGlobal", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "FatItemId" });
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "FatTabelaGlobalId" });
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "FatSubGrupoId" });
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "FatGrupoId" });
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "PlanoId" });
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "ConvenioId" });
            DropIndex("dbo.FatConfigConvenioGlobal", new[] { "EmpresaId" });
            DropTable("dbo.FatConfigConvenioGlobal",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoConfigConvenioGlobal_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
