namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Atendimentocorrecao_FK_OrgUnit_UnidadeOrg : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Atendimento", "UnidadeOrganizacionalId");
            AddForeignKey("dbo.Atendimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Atendimento", "UnidadeOrganizacionalId", "dbo.UnidadeOrganizacional");
            DropIndex("dbo.Atendimento", new[] { "UnidadeOrganizacionalId" });
        }
    }
}
