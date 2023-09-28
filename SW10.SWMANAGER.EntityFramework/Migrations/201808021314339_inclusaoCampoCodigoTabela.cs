namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class inclusaoCampoCodigoTabela : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatContaItem", "CoditoTabela", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FatContaItem", "CoditoTabela");
        }
    }
}
