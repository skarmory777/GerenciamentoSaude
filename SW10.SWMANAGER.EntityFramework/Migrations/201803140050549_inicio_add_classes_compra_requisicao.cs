namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class inicio_add_classes_compra_requisicao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CmpMotivoPedido",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
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
                    { "DynamicFilter_CompraMotivoPedido_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CmpRequisicao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    IsUrgente = c.Boolean(nullable: false),
                    IsAlteraAposGravacao = c.Boolean(nullable: false),
                    IsEncerrada = c.Boolean(nullable: false),
                    IsTipoRequisicaoServico = c.Boolean(nullable: false),
                    IsTipoRequisicaoProduto = c.Boolean(nullable: false),
                    Observacao = c.String(),
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
                    { "DynamicFilter_CompraRequisicao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.CmpRequisicaoItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Quantidade = c.Long(nullable: false),
                    UnidadeId = c.Long(nullable: false),
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
                    { "DynamicFilter_CompraRequisicaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Unidade", t => t.UnidadeId, cascadeDelete: true)
                .Index(t => t.UnidadeId);

        }

        public override void Down()
        {
            DropForeignKey("dbo.CmpRequisicaoItem", "UnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.CmpRequisicaoItem", new[] { "UnidadeId" });
            DropTable("dbo.CmpRequisicaoItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraRequisicaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CmpRequisicao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraRequisicao_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.CmpMotivoPedido",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_CompraMotivoPedido_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
