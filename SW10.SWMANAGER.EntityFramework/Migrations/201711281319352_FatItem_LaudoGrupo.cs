namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class FatItem_LaudoGrupo : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FatItem", "LauGrupoId", c => c.Long());
            CreateIndex("dbo.FatItem", "LauGrupoId");
            AddForeignKey("dbo.FatItem", "LauGrupoId", "dbo.LauGrupo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.FatItem", "LauGrupoId", "dbo.LauGrupo");
            DropIndex("dbo.FatItem", new[] { "LauGrupoId" });
            DropColumn("dbo.FatItem", "LauGrupoId");
        }
    }
}
