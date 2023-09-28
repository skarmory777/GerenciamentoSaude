namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoLancamento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FinLancamento", "NossoNumero", c => c.String());
            AddColumn("dbo.FinLancamento", "CodigoBarras", c => c.String());
            AddColumn("dbo.FinLancamento", "LinhaDigitavel", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FinLancamento", "LinhaDigitavel");
            DropColumn("dbo.FinLancamento", "CodigoBarras");
            DropColumn("dbo.FinLancamento", "NossoNumero");
        }
    }
}
