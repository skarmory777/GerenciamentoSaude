namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Adicao_campos_FormulaEstoque : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.AssFormulaEstoque", name: "EstUnidadeRequisicaoId", newName: "EstUnidadeId");
            RenameIndex(table: "dbo.AssFormulaEstoque", name: "IX_EstUnidadeRequisicaoId", newName: "IX_EstUnidadeId");
        }

        public override void Down()
        {
            RenameIndex(table: "dbo.AssFormulaEstoque", name: "IX_EstUnidadeId", newName: "IX_EstUnidadeRequisicaoId");
            RenameColumn(table: "dbo.AssFormulaEstoque", name: "EstUnidadeId", newName: "EstUnidadeRequisicaoId");
        }
    }
}
