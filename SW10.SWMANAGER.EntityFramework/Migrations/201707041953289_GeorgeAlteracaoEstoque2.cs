namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeAlteracaoEstoque2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoqueMovimento", "EstoqueId", c => c.Long(nullable: false));
            AddColumn("dbo.EstoquePreMovimento", "EstoqueId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstoqueMovimento", "EstoqueId");
            CreateIndex("dbo.EstoquePreMovimento", "EstoqueId");
            AddForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoqueId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoqueId" });
            DropColumn("dbo.EstoquePreMovimento", "EstoqueId");
            DropColumn("dbo.EstoqueMovimento", "EstoqueId");
        }
    }
}
