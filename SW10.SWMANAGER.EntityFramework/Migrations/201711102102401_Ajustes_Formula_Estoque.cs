namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Ajustes_Formula_Estoque : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id", "dbo.AssPrescricaoItem");
            DropForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1", "dbo.AssPrescricaoItem");
            DropForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2", "dbo.AssPrescricaoItem");
            DropIndex("dbo.AssFormulaFaturamento", new[] { "PrescricaoItem_Id" });
            DropIndex("dbo.AssFormulaFaturamento", new[] { "PrescricaoItem_Id1" });
            DropIndex("dbo.AssFormulaFaturamento", new[] { "PrescricaoItem_Id2" });
            DropColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id");
            DropColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1");
            DropColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2");
        }

        public override void Down()
        {
            AddColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2", c => c.Long());
            AddColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1", c => c.Long());
            AddColumn("dbo.AssFormulaFaturamento", "PrescricaoItem_Id", c => c.Long());
            CreateIndex("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2");
            CreateIndex("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1");
            CreateIndex("dbo.AssFormulaFaturamento", "PrescricaoItem_Id");
            AddForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id2", "dbo.AssPrescricaoItem", "Id");
            AddForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id1", "dbo.AssPrescricaoItem", "Id");
            AddForeignKey("dbo.AssFormulaFaturamento", "PrescricaoItem_Id", "dbo.AssPrescricaoItem", "Id");
        }
    }
}
