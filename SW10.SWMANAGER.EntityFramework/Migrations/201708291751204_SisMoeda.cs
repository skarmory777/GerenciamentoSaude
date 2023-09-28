namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class SisMoeda : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SisMoedaCotacaoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisMoedaCotacaoId = c.Long(),
                    ItemId = c.Long(),
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
                    { "DynamicFilter_SisMoedaCotacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.ItemId)
                .ForeignKey("dbo.SisMoedaCotacao", t => t.SisMoedaCotacaoId)
                .Index(t => t.SisMoedaCotacaoId)
                .Index(t => t.ItemId);

            CreateTable(
                "dbo.SisMoedaCotacao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisMoedaId = c.Long(),
                    EmpresaId = c.Long(),
                    ConvenioId = c.Long(),
                    FaturamentoGrupoId = c.Long(),
                    DataInicio = c.DateTime(nullable: false),
                    DataFinal = c.DateTime(nullable: false),
                    Valor = c.Single(nullable: false),
                    IsTodosPlano = c.Boolean(nullable: false),
                    IsTodosItem = c.Boolean(nullable: false),
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
                    { "DynamicFilter_SisMoedaCotacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Convenio", t => t.ConvenioId)
                .ForeignKey("dbo.Empresa", t => t.EmpresaId)
                .ForeignKey("dbo.FatGrupo", t => t.FaturamentoGrupoId)
                .ForeignKey("dbo.SisMoeda", t => t.SisMoedaId)
                .Index(t => t.SisMoedaId)
                .Index(t => t.EmpresaId)
                .Index(t => t.ConvenioId)
                .Index(t => t.FaturamentoGrupoId);

            CreateTable(
                "dbo.SisMoeda",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Codigo = c.String(),
                    Descricao = c.String(),
                    Tipo = c.Int(nullable: false),
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
                    { "DynamicFilter_SisMoeda_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.SisMoedaCotacaoPlano",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    SisMoedaCotacaoId = c.Long(),
                    PlanoId = c.Long(),
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
                    { "DynamicFilter_SisMoedaCotacaoPlano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Plano", t => t.PlanoId)
                .ForeignKey("dbo.SisMoedaCotacao", t => t.SisMoedaCotacaoId)
                .Index(t => t.SisMoedaCotacaoId)
                .Index(t => t.PlanoId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.SisMoedaCotacaoPlano", "SisMoedaCotacaoId", "dbo.SisMoedaCotacao");
            DropForeignKey("dbo.SisMoedaCotacaoPlano", "PlanoId", "dbo.Plano");
            DropForeignKey("dbo.SisMoedaCotacaoItem", "SisMoedaCotacaoId", "dbo.SisMoedaCotacao");
            DropForeignKey("dbo.SisMoedaCotacao", "SisMoedaId", "dbo.SisMoeda");
            DropForeignKey("dbo.SisMoedaCotacao", "FaturamentoGrupoId", "dbo.FatGrupo");
            DropForeignKey("dbo.SisMoedaCotacao", "EmpresaId", "dbo.Empresa");
            DropForeignKey("dbo.SisMoedaCotacao", "ConvenioId", "dbo.Convenio");
            DropForeignKey("dbo.SisMoedaCotacaoItem", "ItemId", "dbo.FatItem");
            DropIndex("dbo.SisMoedaCotacaoPlano", new[] { "PlanoId" });
            DropIndex("dbo.SisMoedaCotacaoPlano", new[] { "SisMoedaCotacaoId" });
            DropIndex("dbo.SisMoedaCotacao", new[] { "FaturamentoGrupoId" });
            DropIndex("dbo.SisMoedaCotacao", new[] { "ConvenioId" });
            DropIndex("dbo.SisMoedaCotacao", new[] { "EmpresaId" });
            DropIndex("dbo.SisMoedaCotacao", new[] { "SisMoedaId" });
            DropIndex("dbo.SisMoedaCotacaoItem", new[] { "ItemId" });
            DropIndex("dbo.SisMoedaCotacaoItem", new[] { "SisMoedaCotacaoId" });
            DropTable("dbo.SisMoedaCotacaoPlano",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisMoedaCotacaoPlano_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMoeda",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisMoeda_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMoedaCotacao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisMoedaCotacao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.SisMoedaCotacaoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_SisMoedaCotacaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
