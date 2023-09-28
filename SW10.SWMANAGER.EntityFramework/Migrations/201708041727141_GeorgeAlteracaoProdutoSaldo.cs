namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoProdutoSaldo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProdutoSaldo", "ConsignadoId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldo", "EmprestimoId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldo", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.ProdutoSaldo", "ValeId", "dbo.Fornecedor");
            DropIndex("dbo.ProdutoSaldo", new[] { "LoteValidadeId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "EmprestimoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "ConsignadoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "ValeId" });
            AlterColumn("dbo.ProdutoSaldo", "LoteValidadeId", c => c.Long());
            AlterColumn("dbo.ProdutoSaldo", "EmprestimoId", c => c.Long());
            AlterColumn("dbo.ProdutoSaldo", "ConsignadoId", c => c.Long());
            AlterColumn("dbo.ProdutoSaldo", "ValeId", c => c.Long());
            CreateIndex("dbo.ProdutoSaldo", "LoteValidadeId");
            CreateIndex("dbo.ProdutoSaldo", "EmprestimoId");
            CreateIndex("dbo.ProdutoSaldo", "ConsignadoId");
            CreateIndex("dbo.ProdutoSaldo", "ValeId");
            AddForeignKey("dbo.ProdutoSaldo", "ConsignadoId", "dbo.Fornecedor", "Id");
            AddForeignKey("dbo.ProdutoSaldo", "EmprestimoId", "dbo.Fornecedor", "Id");
            AddForeignKey("dbo.ProdutoSaldo", "LoteValidadeId", "dbo.LoteValidade", "Id");
            AddForeignKey("dbo.ProdutoSaldo", "ValeId", "dbo.Fornecedor", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.ProdutoSaldo", "ValeId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldo", "LoteValidadeId", "dbo.LoteValidade");
            DropForeignKey("dbo.ProdutoSaldo", "EmprestimoId", "dbo.Fornecedor");
            DropForeignKey("dbo.ProdutoSaldo", "ConsignadoId", "dbo.Fornecedor");
            DropIndex("dbo.ProdutoSaldo", new[] { "ValeId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "ConsignadoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "EmprestimoId" });
            DropIndex("dbo.ProdutoSaldo", new[] { "LoteValidadeId" });
            AlterColumn("dbo.ProdutoSaldo", "ValeId", c => c.Long(nullable: false));
            AlterColumn("dbo.ProdutoSaldo", "ConsignadoId", c => c.Long(nullable: false));
            AlterColumn("dbo.ProdutoSaldo", "EmprestimoId", c => c.Long(nullable: false));
            AlterColumn("dbo.ProdutoSaldo", "LoteValidadeId", c => c.Long(nullable: false));
            CreateIndex("dbo.ProdutoSaldo", "ValeId");
            CreateIndex("dbo.ProdutoSaldo", "ConsignadoId");
            CreateIndex("dbo.ProdutoSaldo", "EmprestimoId");
            CreateIndex("dbo.ProdutoSaldo", "LoteValidadeId");
            AddForeignKey("dbo.ProdutoSaldo", "ValeId", "dbo.Fornecedor", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProdutoSaldo", "LoteValidadeId", "dbo.LoteValidade", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProdutoSaldo", "EmprestimoId", "dbo.Fornecedor", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProdutoSaldo", "ConsignadoId", "dbo.Fornecedor", "Id", cascadeDelete: true);
        }
    }
}
