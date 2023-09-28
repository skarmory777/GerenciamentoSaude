namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_campos_FormulaEstoque_nova_FK : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssFormulaEstoque", "EstUnidadeRequisicaoId", c => c.Long());
            CreateIndex("dbo.AssFormulaEstoque", "EstUnidadeRequisicaoId");
            AddForeignKey("dbo.AssFormulaEstoque", "EstUnidadeRequisicaoId", "dbo.Est_Unidade", "Id", cascadeDelete: false, name: "FX_dbo_AssFormulaEstoque_dbo_Est_Estoque_UnidadeRequisicaoId");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AssFormulaEstoque", "EstUnidadeRequisicaoId", "dbo.Est_Unidade");
            DropIndex("dbo.AssFormulaEstoque", new[] { "EstUnidadeRequisicaoId" });
            DropColumn("dbo.AssFormulaEstoque", "EstUnidadeRequisicaoId");
        }
    }
}
