namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoPessoa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisPessoa", "IsCredito", c => c.Boolean(nullable: false));
            AddColumn("dbo.SisPessoa", "IsDebito", c => c.Boolean(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.SisPessoa", "IsDebito");
            DropColumn("dbo.SisPessoa", "IsCredito");
        }
    }
}
