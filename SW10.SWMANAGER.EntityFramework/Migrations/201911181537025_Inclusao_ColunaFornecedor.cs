namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inclusao_ColunaFornecedor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstImportacaoProduto", "FornecedorId", c => c.Long());
        }
        
        public override void Down()
        {
            DropColumn("dbo.EstImportacaoProduto", "FornecedorId");
        }
    }
}
