namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Fat_ConfigConvenio : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FaturamentoItemFaturamentoKits", newName: "FaturamentoKitFaturamentoItems");
            DropPrimaryKey("dbo.FaturamentoKitFaturamentoItems");
            CreateTable(
                "dbo.FatConfigConvenio",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EmpresaId = c.Long(),
                    ConvenioId = c.Long(),
                    PlanoId = c.Long(),
                    FatGrupoId = c.Long(),
                    FatSubGrupoId = c.Long(),
                    FatTabelaId = c.Long(),
                    FatItemId = c.Long(),
                    DataIncio = c.DateTime(),
                    DataFim = c.DateTime(),
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
                    { "DynamicFilter_FaturamentoConfigConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.ConvenioId)
                .ForeignKey("dbo.SisEmpresa", t => t.EmpresaId)
                .ForeignKey("dbo.FatGrupo", t => t.FatGrupoId)
                .ForeignKey("dbo.FatItem", t => t.FatItemId)
                .ForeignKey("dbo.SisPlano", t => t.PlanoId)
                .ForeignKey("dbo.FatSubGrupo", t => t.FatSubGrupoId)
                .ForeignKey("dbo.FatTabela", t => t.FatTabelaId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ConvenioId)
                .Index(t => t.PlanoId)
                .Index(t => t.FatGrupoId)
                .Index(t => t.FatSubGrupoId)
                .Index(t => t.FatTabelaId)
                .Index(t => t.FatItemId);

            AddPrimaryKey("dbo.FaturamentoKitFaturamentoItems", new[] { "FaturamentoKit_Id", "FaturamentoItem_Id" });
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatConfigConvenio", "FatTabelaId", "dbo.FatTabela");
            DropForeignKey("dbo.FatConfigConvenio", "FatSubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.FatConfigConvenio", "PlanoId", "dbo.SisPlano");
            DropForeignKey("dbo.FatConfigConvenio", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.FatConfigConvenio", "FatGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.FatConfigConvenio", "EmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FatConfigConvenio", "ConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatConfigConvenio", new[] { "FatItemId" });
            DropIndex("dbo.FatConfigConvenio", new[] { "FatTabelaId" });
            DropIndex("dbo.FatConfigConvenio", new[] { "FatSubGrupoId" });
            DropIndex("dbo.FatConfigConvenio", new[] { "FatGrupoId" });
            DropIndex("dbo.FatConfigConvenio", new[] { "PlanoId" });
            DropIndex("dbo.FatConfigConvenio", new[] { "ConvenioId" });
            DropIndex("dbo.FatConfigConvenio", new[] { "EmpresaId" });
            DropPrimaryKey("dbo.FaturamentoKitFaturamentoItems");
            DropTable("dbo.FatConfigConvenio",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FaturamentoConfigConvenio_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            AddPrimaryKey("dbo.FaturamentoKitFaturamentoItems", new[] { "FaturamentoItem_Id", "FaturamentoKit_Id" });
            RenameTable(name: "dbo.FaturamentoKitFaturamentoItems", newName: "FaturamentoItemFaturamentoKits");
        }
    }
}
