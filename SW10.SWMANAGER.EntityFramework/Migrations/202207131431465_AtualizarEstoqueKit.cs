namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AtualizarEstoqueKit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstKit", "ProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.EstKit", new[] { "ProdutoId" });
            AlterColumn("dbo.EstKit", "ProdutoId", c => c.Long());
            CreateIndex("dbo.EstKit", "ProdutoId");
            AddForeignKey("dbo.EstKit", "ProdutoId", "dbo.Est_Produto", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EstKit", "ProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.EstKit", new[] { "ProdutoId" });
            AlterColumn("dbo.EstKit", "ProdutoId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstKit", "ProdutoId");
            AddForeignKey("dbo.EstKit", "ProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
        }
    }
}
