namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class InclusaoCodDependenteAtendimento : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteAtendimento", "CodDependente", c => c.String());
        }

        public override void Down()
        {
            DropColumn("dbo.AteAtendimento", "CodDependente");
        }
    }
}
