namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class mudanca_tipodedados : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.String());
        }

        public override void Down()
        {
            AlterColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.Int());
        }
    }
}
