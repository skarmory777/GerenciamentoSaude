namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class CorrecaoCentroCusto : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CentroCusto", "CodigoCentroCusto", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.CentroCusto", "CodigoCentroCusto", c => c.Int(nullable: false));
        }
    }
}
