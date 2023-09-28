namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class diasautorizacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "DiasAutorizacao", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "DiasAutorizacao");
        }
    }
}
