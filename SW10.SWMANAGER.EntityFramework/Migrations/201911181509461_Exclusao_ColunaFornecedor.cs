namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exclusao_ColunaFornecedor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstImportacaoProduto", "FK_dbo.EstImportacaoProduto_dbo.Fornecedor_FornecedorId");
            DropIndex("dbo.EstImportacaoProduto", new[] { "FornecedorId" });
            AddColumn("dbo.Est_Produto", "LaboratorioId", c => c.Long());
            CreateIndex("dbo.Est_Produto", "LaboratorioId");
            AddForeignKey("dbo.Est_Produto", "LaboratorioId", "dbo.EstLaboratorio", "Id");
            DropColumn("dbo.EstImportacaoProduto", "FornecedorId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.EstImportacaoProduto", "FornecedorId", c => c.Long(nullable: false));
            DropForeignKey("dbo.Est_Produto", "LaboratorioId", "dbo.EstLaboratorio");
            DropIndex("dbo.Est_Produto", new[] { "LaboratorioId" });
            DropColumn("dbo.Est_Produto", "LaboratorioId");
            CreateIndex("dbo.EstImportacaoProduto", "FornecedorId");
            AddForeignKey("dbo.EstImportacaoProduto", "FornecedorId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }
    }
}
