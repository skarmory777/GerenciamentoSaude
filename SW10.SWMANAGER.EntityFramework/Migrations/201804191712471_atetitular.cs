namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class atetitular : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "Titular", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "Titular");
        }
    }
}
