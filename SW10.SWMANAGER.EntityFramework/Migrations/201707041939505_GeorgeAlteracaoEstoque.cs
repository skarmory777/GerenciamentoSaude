namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoEstoque : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.ProdutoEstoque");
            DropForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.ProdutoEstoque");
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoqueId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoqueId" });
            DropColumn("dbo.EstoqueMovimento", "EstoqueId");
            DropColumn("dbo.EstoquePreMovimento", "EstoqueId");
        }

        public override void Down()
        {
            AddColumn("dbo.EstoquePreMovimento", "EstoqueId", c => c.Long(nullable: false));
            AddColumn("dbo.EstoqueMovimento", "EstoqueId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstoquePreMovimento", "EstoqueId");
            CreateIndex("dbo.EstoqueMovimento", "EstoqueId");
            AddForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.ProdutoEstoque", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.ProdutoEstoque", "Id", cascadeDelete: true);
        }
    }
}
