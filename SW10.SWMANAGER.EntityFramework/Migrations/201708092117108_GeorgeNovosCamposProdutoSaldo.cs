namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class GeorgeNovosCamposProdutoSaldo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProdutoSaldo", "QuantidadeEntradaPendente", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ProdutoSaldo", "QuantidadeSaidaPendente", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }

        public override void Down()
        {
            DropColumn("dbo.ProdutoSaldo", "QuantidadeSaidaPendente");
            DropColumn("dbo.ProdutoSaldo", "QuantidadeEntradaPendente");
        }
    }
}
