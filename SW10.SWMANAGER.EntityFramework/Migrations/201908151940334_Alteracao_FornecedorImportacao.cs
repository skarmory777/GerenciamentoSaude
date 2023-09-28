namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Alteracao_FornecedorImportacao : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.EstImportacaoProduto", "FornecedorId");
            AddForeignKey("dbo.EstImportacaoProduto", "FornecedorId", "dbo.SisFornecedor", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstImportacaoProduto", "FornecedorId", "dbo.SisFornecedor");
            DropIndex("dbo.EstImportacaoProduto", new[] { "FornecedorId" });
        }
    }
}
