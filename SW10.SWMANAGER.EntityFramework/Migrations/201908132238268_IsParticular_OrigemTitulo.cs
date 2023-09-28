namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class IsParticular_OrigemTitulo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "IsParticular", c => c.Boolean(nullable: false));
            AddColumn("dbo.AteAtendimento", "OrigemTitular", c => c.Int(nullable: false));
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "OrigemTitular");
            DropColumn("dbo.SisConvenio", "IsParticular");
        }
    }
}
