namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class ate_parentesco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "Parentesco", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "Parentesco");
        }
    }
}
