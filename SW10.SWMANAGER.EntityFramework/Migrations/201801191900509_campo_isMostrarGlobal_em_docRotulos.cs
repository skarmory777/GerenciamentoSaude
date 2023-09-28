namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class campo_isMostrarGlobal_em_docRotulos : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocRotulo", "IsMostrarGlobal", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.DocRotulo", "IsMostrarGlobal");
        }
    }
}
