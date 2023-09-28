namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeInclusaoUnidade_ImportacaoProduto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstImportacaoProduto", "UnidadeId", c => c.Long(nullable: false));
            AddColumn("dbo.EstImportacaoProduto", "Fator", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            CreateIndex("dbo.EstImportacaoProduto", "UnidadeId");
            AddForeignKey("dbo.EstImportacaoProduto", "UnidadeId", "dbo.Est_Unidade", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            DropForeignKey("dbo.EstImportacaoProduto", "UnidadeId", "dbo.Est_Unidade");
            DropIndex("dbo.EstImportacaoProduto", new[] { "UnidadeId" });
            DropColumn("dbo.EstImportacaoProduto", "Fator");
            DropColumn("dbo.EstImportacaoProduto", "UnidadeId");
        }
    }
}
