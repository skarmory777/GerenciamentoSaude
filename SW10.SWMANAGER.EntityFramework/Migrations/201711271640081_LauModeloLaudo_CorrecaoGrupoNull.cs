namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class LauModeloLaudo_CorrecaoGrupoNull : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LauModeloLaudo", "LauGrupoId", "dbo.LauGrupo");
            DropIndex("dbo.LauModeloLaudo", new[] { "LauGrupoId" });
            AlterColumn("dbo.LauModeloLaudo", "LauGrupoId", c => c.Long());
            CreateIndex("dbo.LauModeloLaudo", "LauGrupoId");
            AddForeignKey("dbo.LauModeloLaudo", "LauGrupoId", "dbo.LauGrupo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.LauModeloLaudo", "LauGrupoId", "dbo.LauGrupo");
            DropIndex("dbo.LauModeloLaudo", new[] { "LauGrupoId" });
            AlterColumn("dbo.LauModeloLaudo", "LauGrupoId", c => c.Long(nullable: false));
            CreateIndex("dbo.LauModeloLaudo", "LauGrupoId");
            AddForeignKey("dbo.LauModeloLaudo", "LauGrupoId", "dbo.LauGrupo", "Id", cascadeDelete: true);
        }
    }
}
