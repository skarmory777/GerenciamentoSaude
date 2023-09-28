namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Fat_Conta_MotivoPendencia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatConta", "MotivoPendencia", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.FatConta", "MotivoPendencia");
        }
    }
}
