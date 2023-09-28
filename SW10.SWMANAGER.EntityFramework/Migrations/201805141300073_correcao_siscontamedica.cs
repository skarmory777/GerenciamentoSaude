namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class correcao_siscontamedica : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.Long());
        }

        public override void Down()
        {
            AlterColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.String());
        }
    }
}
