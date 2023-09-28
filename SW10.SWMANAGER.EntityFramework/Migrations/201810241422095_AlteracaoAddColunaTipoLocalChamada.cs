namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoAddColunaTipoLocalChamada : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AteTipoLocalChamada", "UnidadeOrganizacionalId", c => c.Long());
            CreateIndex("dbo.AteTipoLocalChamada", "UnidadeOrganizacionalId");
            AddForeignKey("dbo.AteTipoLocalChamada", "UnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.AteTipoLocalChamada", "UnidadeOrganizacionalId", "dbo.SisUnidadeOrganizacional");
            DropIndex("dbo.AteTipoLocalChamada", new[] { "UnidadeOrganizacionalId" });
            DropColumn("dbo.AteTipoLocalChamada", "UnidadeOrganizacionalId");
        }
    }
}
