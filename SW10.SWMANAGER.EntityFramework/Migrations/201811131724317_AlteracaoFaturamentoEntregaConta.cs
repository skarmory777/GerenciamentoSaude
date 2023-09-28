namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoFaturamentoEntregaConta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatEntregaLote", "IsLoteGerado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FatEntregaLote", "IsLoteGerado");
        }
    }
}
