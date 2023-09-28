namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class correcao_diasautorizacao : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AteAtendimento", "DiasAutorizacao");
        }

        public override void Down()
        {
            AddColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.DateTime());
        }
    }
}
