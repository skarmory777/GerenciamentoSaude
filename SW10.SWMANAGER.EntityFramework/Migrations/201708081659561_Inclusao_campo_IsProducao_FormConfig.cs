namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Inclusao_campo_IsProducao_FormConfig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FormConfig", "IsProducao", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.FormConfig", "IsProducao");
        }
    }
}
