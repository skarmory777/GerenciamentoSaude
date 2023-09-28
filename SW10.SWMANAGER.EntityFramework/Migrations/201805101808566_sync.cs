namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class sync : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Sis_Internacao", "DataPrevisaoAlta", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.Sis_Internacao", "DataPrevisaoAlta", c => c.DateTime(nullable: false));
        }
    }
}
