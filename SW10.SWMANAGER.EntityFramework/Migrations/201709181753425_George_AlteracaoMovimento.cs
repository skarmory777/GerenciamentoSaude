namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class George_AlteracaoMovimento : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoqueId" });
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoqueId" });
            AlterColumn("dbo.EstoqueMovimento", "EstoqueId", c => c.Long());
            AlterColumn("dbo.EstoquePreMovimento", "EstoqueId", c => c.Long());
            CreateIndex("dbo.EstoqueMovimento", "EstoqueId");
            CreateIndex("dbo.EstoquePreMovimento", "EstoqueId");
            AddForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.Est_Estoque", "Id");
            AddForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.Est_Estoque", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.Est_Estoque");
            DropForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.Est_Estoque");
            DropIndex("dbo.EstoquePreMovimento", new[] { "EstoqueId" });
            DropIndex("dbo.EstoqueMovimento", new[] { "EstoqueId" });
            AlterColumn("dbo.EstoquePreMovimento", "EstoqueId", c => c.Long(nullable: false));
            AlterColumn("dbo.EstoqueMovimento", "EstoqueId", c => c.Long(nullable: false));
            CreateIndex("dbo.EstoquePreMovimento", "EstoqueId");
            CreateIndex("dbo.EstoqueMovimento", "EstoqueId");
            AddForeignKey("dbo.EstoquePreMovimento", "EstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
            AddForeignKey("dbo.EstoqueMovimento", "EstoqueId", "dbo.Est_Estoque", "Id", cascadeDelete: true);
        }
    }
}
