namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Incluindo_ImportaLocalUtilizacaoId_UnidadeOrganizacional : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SisUnidadeOrganizacional", "ImportaLocalUtilizacaoId", c => c.Int());
        }

        public override void Down()
        {
            DropColumn("dbo.SisUnidadeOrganizacional", "ImportaLocalUtilizacaoId");
        }
    }
}
