namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoCampoCodigoTabela : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "CodigoTabela", c => c.String());
            DropColumn("dbo.FatContaItem", "CoditoTabela");
        }

        public override void Down()
        {
            AddColumn("dbo.FatContaItem", "CoditoTabela", c => c.String());
            DropColumn("dbo.FatContaItem", "CodigoTabela");
        }
    }
}
