namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoConveio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisConvenio", "RegistroANS", c => c.String());
            AlterColumn("dbo.SisPessoa", "Nascimento", c => c.DateTime());
        }

        public override void Down()
        {
            AlterColumn("dbo.SisPessoa", "Nascimento", c => c.DateTime(nullable: false));
            DropColumn("dbo.SisConvenio", "RegistroANS");
        }
    }
}
