namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoTabela : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LabResultadoLaudo", "Formula", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.LabResultadoLaudo", "Formula");
        }
    }
}
