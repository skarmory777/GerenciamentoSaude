namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class TipoMovimento_add_columns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EstTipoMovimento", "IsOrdemCompra", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstTipoMovimento", "IsPessoa", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstTipoMovimento", "IsOrdemCompraObrigatoria", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstTipoMovimento", "IsFiscal", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstTipoMovimento", "IsFrete", c => c.Boolean(nullable: false));
            AddColumn("dbo.EstTipoMovimento", "IsFinanceiro", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.EstTipoMovimento", "IsFinanceiro");
            DropColumn("dbo.EstTipoMovimento", "IsFrete");
            DropColumn("dbo.EstTipoMovimento", "IsFiscal");
            DropColumn("dbo.EstTipoMovimento", "IsOrdemCompraObrigatoria");
            DropColumn("dbo.EstTipoMovimento", "IsPessoa");
            DropColumn("dbo.EstTipoMovimento", "IsOrdemCompra");
        }
    }
}
