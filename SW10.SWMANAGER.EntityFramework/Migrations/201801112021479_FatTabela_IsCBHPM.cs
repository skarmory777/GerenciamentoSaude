namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatTabela_IsCBHPM : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatTabela", "IsCBHPM", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FatTabela", "IsCBHPM");
        }
    }
}
