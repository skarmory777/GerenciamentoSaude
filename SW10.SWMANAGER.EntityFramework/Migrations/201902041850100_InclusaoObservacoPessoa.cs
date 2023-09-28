namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoObservacoPessoa : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisPessoa", "Observacao", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.SisPessoa", "Observacao");
        }
    }
}
