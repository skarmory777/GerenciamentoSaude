namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Medico_IsIndeterminado : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisMedico", "IsIndeterminado", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.SisMedico", "IsIndeterminado");
        }
    }
}
