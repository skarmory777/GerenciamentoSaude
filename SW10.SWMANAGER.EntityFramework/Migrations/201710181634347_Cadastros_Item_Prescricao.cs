namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Cadastros_Item_Prescricao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssFormulaEstoque",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    EstoqueOrigemId = c.Long(),
                    EstProdutoId = c.Long(),
                    EstUnidadeRequisicaoId = c.Long(),
                    AssPrescricaoItemId = c.Long(nullable: false),
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
                    { "DynamicFilter_FormulaEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Est_Estoque", t => t.EstoqueOrigemId)
                .ForeignKey("dbo.ItemPrescricao", t => t.AssPrescricaoItemId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.EstProdutoId)
                .ForeignKey("dbo.Est_Unidade", t => t.EstUnidadeRequisicaoId)
                .Index(t => t.EstoqueOrigemId)
                .Index(t => t.EstProdutoId)
                .Index(t => t.EstUnidadeRequisicaoId)
                .Index(t => t.AssPrescricaoItemId);

            CreateTable(
                "dbo.AssFormulaEstoqueItem",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    EstProdutoId = c.Long(),
                    EstUnidadeId = c.Long(),
                    Quantidade = c.Decimal(nullable: false, precision: 18, scale: 2),
                    IsVisivel = c.Boolean(nullable: false),
                    IsGeraSolicitacaoEstoque = c.Boolean(nullable: false),
                    Descricao = c.String(),
                    AssFormulaEstoqueId = c.Long(nullable: false),
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
                    { "DynamicFilter_FormulaEstoqueItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssFormulaEstoque", t => t.AssFormulaEstoqueId, cascadeDelete: false)
                .ForeignKey("dbo.Est_Produto", t => t.EstProdutoId)
                .ForeignKey("dbo.Est_Unidade", t => t.EstUnidadeId)
                .Index(t => t.EstProdutoId)
                .Index(t => t.EstUnidadeId)
                .Index(t => t.AssFormulaEstoqueId);

            CreateTable(
                "dbo.ItemPrescricao",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    AssDivisaoId = c.Long(),
                    IsAtivo = c.Boolean(nullable: false),
                    IsAlertaDuplicidade = c.Boolean(nullable: false),
                    IsExigeJustificativa = c.Boolean(nullable: false),
                    Justificativa = c.String(),
                    AssTipoPrescricaoId = c.Long(),
                    AssTipoControleId = c.Long(),
                    IsAlteraQuantidade = c.Boolean(nullable: false),
                    DosePadrao = c.Decimal(nullable: false, precision: 18, scale: 2),
                    EstUnidadeId = c.Long(),
                    AssFormaAplicacaoId = c.Long(),
                    AssFrequenciaId = c.Long(),
                    Duracao = c.String(),
                    AssVelocidadeInfusaoId = c.Long(),
                    EstUnidadeInfusaoId = c.Long(),
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
                    { "DynamicFilter_PrescricaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AssDivisao", t => t.AssDivisaoId)
                .ForeignKey("dbo.AssFormaAplicacao", t => t.AssFormaAplicacaoId)
                .ForeignKey("dbo.AssFrequencia", t => t.AssFrequenciaId)
                .ForeignKey("dbo.TipoControle", t => t.AssTipoControleId)
                .ForeignKey("dbo.AssTipoPrescricao", t => t.AssTipoPrescricaoId)
                .ForeignKey("dbo.Est_Unidade", t => t.EstUnidadeId)
                .ForeignKey("dbo.Est_Unidade", t => t.EstUnidadeInfusaoId)
                .ForeignKey("dbo.AssVelocidadeInfusao", t => t.AssVelocidadeInfusaoId)
                .Index(t => t.AssDivisaoId)
                .Index(t => t.AssTipoPrescricaoId)
                .Index(t => t.AssTipoControleId)
                .Index(t => t.EstUnidadeId)
                .Index(t => t.AssFormaAplicacaoId)
                .Index(t => t.AssFrequenciaId)
                .Index(t => t.AssVelocidadeInfusaoId)
                .Index(t => t.EstUnidadeInfusaoId);

            CreateTable(
                "dbo.AssFormulaExameLaboratorial",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    FatItemId = c.Long(),
                    LabMaterialId = c.Long(),
                    AssPrescricaoItemId = c.Long(),
                    IsSistema = c.Boolean(nullable: false),
                    Codigo = c.String(maxLength: 10),
                    IsDeleted = c.Boolean(nullable: false),
                    DeleterUserId = c.Long(),
                    DeletionTime = c.DateTime(),
                    LastModificationTime = c.DateTime(),
                    LastModifierUserId = c.Long(),
                    CreationTime = c.DateTime(nullable: false),
                    CreatorUserId = c.Long(),
                    PrescricaoItem_Id = c.Long(),
                    PrescricaoItem_Id1 = c.Long(),
                },
                annotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.FatItemId)
                .ForeignKey("dbo.LabMaterial", t => t.LabMaterialId)
                .ForeignKey("dbo.ItemPrescricao", t => t.AssPrescricaoItemId)
                .ForeignKey("dbo.ItemPrescricao", t => t.PrescricaoItem_Id)
                .ForeignKey("dbo.ItemPrescricao", t => t.PrescricaoItem_Id1)
                .Index(t => t.FatItemId)
                .Index(t => t.LabMaterialId)
                .Index(t => t.AssPrescricaoItemId)
                .Index(t => t.PrescricaoItem_Id)
                .Index(t => t.PrescricaoItem_Id1);

            CreateTable(
                "dbo.AssFormulaFaturamento",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
                    FatItem = c.Long(),
                    IsFatura = c.Boolean(nullable: false),
                    AssPrescricaoItemId = c.Long(nullable: false),
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
                    { "DynamicFilter_FormulaFaturamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FatItem", t => t.FatItem)
                .ForeignKey("dbo.ItemPrescricao", t => t.AssPrescricaoItemId, cascadeDelete: false)
                .Index(t => t.FatItem)
                .Index(t => t.AssPrescricaoItemId);

            CreateTable(
                "dbo.TipoControle",
                c => new
                {
                    Id = c.Long(nullable: false, identity: true),
                    Descricao = c.String(),
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
                    { "DynamicFilter_TipoControle_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.AssFormulaEstoque", "EstUnidadeRequisicaoId", "dbo.Est_Unidade");
            DropForeignKey("dbo.AssFormulaEstoque", "EstProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.AssFormulaEstoque", "AssPrescricaoItemId", "dbo.ItemPrescricao");
            DropForeignKey("dbo.ItemPrescricao", "AssVelocidadeInfusaoId", "dbo.AssVelocidadeInfusao");
            DropForeignKey("dbo.ItemPrescricao", "EstUnidadeInfusaoId", "dbo.Est_Unidade");
            DropForeignKey("dbo.ItemPrescricao", "EstUnidadeId", "dbo.Est_Unidade");
            DropForeignKey("dbo.ItemPrescricao", "AssTipoPrescricaoId", "dbo.AssTipoPrescricao");
            DropForeignKey("dbo.ItemPrescricao", "AssTipoControleId", "dbo.TipoControle");
            DropForeignKey("dbo.ItemPrescricao", "AssFrequenciaId", "dbo.AssFrequencia");
            DropForeignKey("dbo.AssFormulaFaturamento", "AssPrescricaoItemId", "dbo.ItemPrescricao");
            DropForeignKey("dbo.AssFormulaFaturamento", "FatItem", "dbo.FatItem");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "PrescricaoItem_Id1", "dbo.ItemPrescricao");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "PrescricaoItem_Id", "dbo.ItemPrescricao");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "AssPrescricaoItemId", "dbo.ItemPrescricao");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "LabMaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.ItemPrescricao", "AssFormaAplicacaoId", "dbo.AssFormaAplicacao");
            DropForeignKey("dbo.ItemPrescricao", "AssDivisaoId", "dbo.AssDivisao");
            DropForeignKey("dbo.AssFormulaEstoqueItem", "EstUnidadeId", "dbo.Est_Unidade");
            DropForeignKey("dbo.AssFormulaEstoqueItem", "EstProdutoId", "dbo.Est_Produto");
            DropForeignKey("dbo.AssFormulaEstoqueItem", "AssFormulaEstoqueId", "dbo.AssFormulaEstoque");
            DropForeignKey("dbo.AssFormulaEstoque", "EstoqueOrigemId", "dbo.Est_Estoque");
            DropIndex("dbo.AssFormulaFaturamento", new[] { "AssPrescricaoItemId" });
            DropIndex("dbo.AssFormulaFaturamento", new[] { "FatItem" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "PrescricaoItem_Id1" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "PrescricaoItem_Id" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "AssPrescricaoItemId" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "LabMaterialId" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "FatItemId" });
            DropIndex("dbo.ItemPrescricao", new[] { "EstUnidadeInfusaoId" });
            DropIndex("dbo.ItemPrescricao", new[] { "AssVelocidadeInfusaoId" });
            DropIndex("dbo.ItemPrescricao", new[] { "AssFrequenciaId" });
            DropIndex("dbo.ItemPrescricao", new[] { "AssFormaAplicacaoId" });
            DropIndex("dbo.ItemPrescricao", new[] { "EstUnidadeId" });
            DropIndex("dbo.ItemPrescricao", new[] { "AssTipoControleId" });
            DropIndex("dbo.ItemPrescricao", new[] { "AssTipoPrescricaoId" });
            DropIndex("dbo.ItemPrescricao", new[] { "AssDivisaoId" });
            DropIndex("dbo.AssFormulaEstoqueItem", new[] { "AssFormulaEstoqueId" });
            DropIndex("dbo.AssFormulaEstoqueItem", new[] { "EstUnidadeId" });
            DropIndex("dbo.AssFormulaEstoqueItem", new[] { "EstProdutoId" });
            DropIndex("dbo.AssFormulaEstoque", new[] { "AssPrescricaoItemId" });
            DropIndex("dbo.AssFormulaEstoque", new[] { "EstUnidadeRequisicaoId" });
            DropIndex("dbo.AssFormulaEstoque", new[] { "EstProdutoId" });
            DropIndex("dbo.AssFormulaEstoque", new[] { "EstoqueOrigemId" });
            DropTable("dbo.TipoControle",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_TipoControle_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssFormulaFaturamento",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaFaturamento_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssFormulaExameLaboratorial",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.ItemPrescricao",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_PrescricaoItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssFormulaEstoqueItem",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaEstoqueItem_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
            DropTable("dbo.AssFormulaEstoque",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaEstoque_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }
    }
}
