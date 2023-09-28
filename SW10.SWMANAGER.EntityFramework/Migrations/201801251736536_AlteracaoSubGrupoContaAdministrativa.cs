namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AlteracaoSubGrupoContaAdministrativa : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", "dbo.FinGrupoContaAdministrativa");
            DropIndex("dbo.FinSubGrupoContaAdministrativa", new[] { "GrupoContaAdministrativaId" });
            AlterColumn("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", c => c.Long());
            CreateIndex("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId");
            AddForeignKey("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", "dbo.FinGrupoContaAdministrativa", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", "dbo.FinGrupoContaAdministrativa");
            DropIndex("dbo.FinSubGrupoContaAdministrativa", new[] { "GrupoContaAdministrativaId" });
            AlterColumn("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", c => c.Long(nullable: false));
            CreateIndex("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId");
            AddForeignKey("dbo.FinSubGrupoContaAdministrativa", "GrupoContaAdministrativaId", "dbo.FinGrupoContaAdministrativa", "Id", cascadeDelete: true);
        }
    }
}
