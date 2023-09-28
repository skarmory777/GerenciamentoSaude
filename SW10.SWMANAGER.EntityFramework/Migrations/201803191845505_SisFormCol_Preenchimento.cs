namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SisFormCol_Preenchimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormColConfig", "Preenchimento", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.SisFormColConfig", "Preenchimento");
        }
    }
}
