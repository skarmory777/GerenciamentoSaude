namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCampoSequencial : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisFormData", "Sequencial", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.SisFormData", "Sequencial");
        }
    }
}
