namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Incluindo_FatCodigoCredenciado_FatTabelaConvenioCodigo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FatCodigoCredenciado",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsAmbulatorioEmergencia = c.Boolean(nullable: false),
                    IsInternacao = c.Boolean(nullable: false),
                    IsFuturoEspecialidade = c.Boolean(nullable: false),
                    SisConvenioId = c.Long(),
                    SisEmpresaId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_CodigoCredenciado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId)
                .ForeignKey("dbo.SisEmpresa", t => t.SisEmpresaId)
                .Index(t => t.SisConvenioId)
                .Index(t => t.SisEmpresaId);

            CreateTable(
                "dbo.FatTabelaConvenioCodigo",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisConvenioId = c.Long(nullable: false),
                    FatTabelaPrecoItemId = c.Long(nullable: false),
                    IsFromTuss = c.Boolean(nullable: false),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    Descricao = c.String(),
                    ImportaId = c.Int(),
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
                    { "DynamicFilter_TabelaConvenioCodigo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => new { t.Id, t.SisConvenioId, t.FatTabelaPrecoItemId })
                .ForeignKey("dbo.SisConvenio", t => t.SisConvenioId, cascadeDelete: false)
                .ForeignKey("dbo.FatTabelaPrecoItem", t => t.FatTabelaPrecoItemId, cascadeDelete: false)
                .Index(t => t.SisConvenioId)
                .Index(t => t.FatTabelaPrecoItemId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.FatTabelaConvenioCodigo", "FatTabelaPrecoItemId", "dbo.FatTabelaPrecoItem");
            DropForeignKey("dbo.FatTabelaConvenioCodigo", "SisConvenioId", "dbo.SisConvenio");
            DropForeignKey("dbo.FatCodigoCredenciado", "SisEmpresaId", "dbo.SisEmpresa");
            DropForeignKey("dbo.FatCodigoCredenciado", "SisConvenioId", "dbo.SisConvenio");
            DropIndex("dbo.FatTabelaConvenioCodigo", new[] { "FatTabelaPrecoItemId" });
            DropIndex("dbo.FatTabelaConvenioCodigo", new[] { "SisConvenioId" });
            DropIndex("dbo.FatCodigoCredenciado", new[] { "SisEmpresaId" });
            DropIndex("dbo.FatCodigoCredenciado", new[] { "SisConvenioId" });
            DropTable("dbo.FatTabelaConvenioCodigo",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TabelaConvenioCodigo_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.FatCodigoCredenciado",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CodigoCredenciado_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
