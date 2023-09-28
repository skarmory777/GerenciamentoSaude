namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UnidadeOrganizacional_ControlaAlta : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnidadeOrganizacional", "ControlaAlta", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UnidadeOrganizacional", "ControlaAlta");
        }
    }
}
