namespace SW10.SWMANAGER.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Atualizando_OrdemCompra : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpOrdemCompraItem", "EstProdutoId", c => c.Long(nullable: false));
            CreateIndex("dbo.CmpOrdemCompraItem", "EstProdutoId");
            AddForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CmpOrdemCompraItem", "EstProdutoId", "dbo.Est_Produto");
            DropIndex("dbo.CmpOrdemCompraItem", new[] { "EstProdutoId" });
            DropColumn("dbo.CmpOrdemCompraItem", "EstProdutoId");
        }
    }
}
