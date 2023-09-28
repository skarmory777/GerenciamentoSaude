namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Atendimentocorrecao_FK_OrgUnit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Atendimento", "UnidadeOrganizacionalId", "dbo.AbpOrganizationUnits");
            DropIndex("dbo.Atendimento", new[] { "UnidadeOrganizacionalId" });
        }

        public override void Down()
        {
            CreateIndex("dbo.Atendimento", "UnidadeOrganizacionalId");
            AddForeignKey("dbo.Atendimento", "UnidadeOrganizacionalId", "dbo.AbpOrganizationUnits", "Id");
        }
    }
}
