namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCamposKitItem_FormulaEstoque : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstKitItem", "UnidadeId", c => c.Long(nullable: false));
            AddColumn("dbo.AssFormulaEstoque", "Quantidade", c => c.Int(nullable: false));
            CreateIndex("dbo.EstKitItem", "UnidadeId");
            AddForeignKey("dbo.EstKitItem", "UnidadeId", "dbo.Est_Unidade", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstKitItem", "UnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.EstKitItem", new[] { "UnidadeId" });
            DropColumn("dbo.AssFormulaEstoque", "Quantidade");
            DropColumn("dbo.EstKitItem", "UnidadeId");
        }
    }
}
