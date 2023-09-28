namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class UnidadeOrganizacional_SetorExames : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UnidadeOrganizacional", "IsSetorExames", c => c.Boolean(nullable: false));
            AddColumn("dbo.UnidadeOrganizacional", "IsEstoque", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.UnidadeOrganizacional", "IsEstoque");
            DropColumn("dbo.UnidadeOrganizacional", "IsSetorExames");
        }
    }
}
