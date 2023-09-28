namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class DocRotulo_Prioridade : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DocRotulo", "IsPrioridade", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.DocRotulo", "IsPrioridade");
        }
    }
}
