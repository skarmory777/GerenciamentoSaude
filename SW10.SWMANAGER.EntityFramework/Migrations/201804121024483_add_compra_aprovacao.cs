namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_compra_aprovacao : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CmpRequisicao", "IsAprovada", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.CmpRequisicao", "IsAprovada");
        }
    }
}
