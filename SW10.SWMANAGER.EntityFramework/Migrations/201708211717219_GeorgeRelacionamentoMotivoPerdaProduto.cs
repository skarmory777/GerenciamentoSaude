namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeRelacionamentoMotivoPerdaProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstoquePreMovimento", "MotivoPerdaProdutoId", c => c.Long());
            CreateIndex("dbo.EstoquePreMovimento", "MotivoPerdaProdutoId");
            AddForeignKey("dbo.EstoquePreMovimento", "MotivoPerdaProdutoId", "dbo.MotivoPerdaProduto", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstoquePreMovimento", "MotivoPerdaProdutoId", "dbo.MotivoPerdaProduto");
            DropIndex("dbo.EstoquePreMovimento", new[] { "MotivoPerdaProdutoId" });
            DropColumn("dbo.EstoquePreMovimento", "MotivoPerdaProdutoId");
        }
    }
}
