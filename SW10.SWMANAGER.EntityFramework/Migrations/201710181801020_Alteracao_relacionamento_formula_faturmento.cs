namespace SW10.SWMANAGER.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;

    public partial class Alteracao_relacionamento_formula_faturmento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "FatItemId", "dbo.FatItem");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "LabMaterialId", "dbo.LabMaterial");
            DropForeignKey("dbo.AssFormulaExameLaboratorial", "AssPrescricaoItemId", "dbo.ItemPrescricao");
            DropForeignKey("dbo.AssFormulaFaturamento", "AssPrescricaoItemId", "dbo.ItemPrescricao");
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "FatItemId" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "LabMaterialId" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "AssPrescricaoItemId" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "PrescricaoItem_Id" });
            DropIndex("dbo.AssFormulaExameLaboratorial", new[] { "PrescricaoItem_Id1" });
            AddColumn("dbo.AssFormulaFaturamento", "LabMaterialId", c => c.Long());
            AddColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id", c => c.Long());
            AddColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1", c => c.Long());
            AddColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2", c => c.Long());
            CreateIndex("dbo.AssFormulaFaturamento", "LabMaterialId");
            CreateIndex("dbo.AssFormulaFaturamento", "PrescricaoItem_Id");
            CreateIndex("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1");
            CreateIndex("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2");
            AddForeignKey("dbo.AssFormulaFaturamento", "LabMaterialId", "dbo.LabMaterial", "Id");
            AddForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2", "dbo.ItemPrescricao", "Id");
            DropTable("dbo.AssFormulaExameLaboratorial",
                removedAnnotations: new Dictionary<string, object>
                {
                    { "DynamicFilter_FormulaExame_SoftDelete", "EntityFramework.DynamicFilters.DynamicFilterDefinition" },
                });
        }

        public override void Down()
        {
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
                .PrimaryKey(t => t.Id);

            DropForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2", "dbo.ItemPrescricao");
            DropForeignKey("dbo.AssFormulaFaturamento", "LabMaterialId", "dbo.LabMaterial");
            DropIndex("dbo.AssFormulaFaturamento", new[] { "PrescricaoItem_Id2" });
            DropIndex("dbo.AssFormulaFaturamento", new[] { "PrescricaoItem_Id1" });
            DropIndex("dbo.AssFormulaFaturamento", new[] { "PrescricaoItem_Id" });
            DropIndex("dbo.AssFormulaFaturamento", new[] { "LabMaterialId" });
            DropColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2");
            DropColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1");
            DropColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id");
            DropColumn("dbo.AssFormulaFaturamento", "LabMaterialId");
            CreateIndex("dbo.AssFormulaExameLaboratorial", "PrescricaoItem_Id1");
            CreateIndex("dbo.AssFormulaExameLaboratorial", "PrescricaoItem_Id");
            CreateIndex("dbo.AssFormulaExameLaboratorial", "AssPrescricaoItemId");
            CreateIndex("dbo.AssFormulaExameLaboratorial", "LabMaterialId");
            CreateIndex("dbo.AssFormulaExameLaboratorial", "FatItemId");
            AddForeignKey("dbo.AssFormulaFaturamento", "AssPrescricaoItemId", "dbo.ItemPrescricao", "Id", cascadeDelete: false);
            AddForeignKey("dbo.AssFormulaExameLaboratorial", "AssPrescricaoItemId", "dbo.ItemPrescricao", "Id");
            AddForeignKey("dbo.AssFormulaExameLaboratorial", "LabMaterialId", "dbo.LabMaterial", "Id");
            AddForeignKey("dbo.AssFormulaExameLaboratorial", "FatItemId", "dbo.FatItem", "Id");
        }
    }
}
