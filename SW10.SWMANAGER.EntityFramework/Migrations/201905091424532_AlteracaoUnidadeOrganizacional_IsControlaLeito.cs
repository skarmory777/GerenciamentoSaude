namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoUnidadeOrganizacional_IsControlaLeito : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisUnidadeOrganizacional", "IsControlaLeito", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.SisUnidadeOrganizacional", "IsControlaLeito");
        }
    }
}
