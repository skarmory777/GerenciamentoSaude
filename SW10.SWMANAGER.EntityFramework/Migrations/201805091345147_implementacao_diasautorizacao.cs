namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class implementacao_diasautorizacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.DateTime());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "DiasAutorizacao");
        }
    }
}
