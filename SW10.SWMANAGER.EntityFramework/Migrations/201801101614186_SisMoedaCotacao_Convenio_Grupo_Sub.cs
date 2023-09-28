namespace SW10.SWMANAGER.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class SisMoedaCotacao_Convenio_Grupo_Sub : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.SisMoedaCotacao", name: "FaturamentoGrupoId", newName: "GrupoId");
            RenameIndex(table: "dbo.SisMoedaCotacao", name: "IX_FaturamentoGrupoId", newName: "IX_GrupoId");
            AddColumn("dbo.SisMoedaCotacao", "PlanoId", c => c.Long());
            AddColumn("dbo.SisMoedaCotacao", "SubGrupoId", c => c.Long());
            CreateIndex("dbo.SisMoedaCotacao", "PlanoId");
            CreateIndex("dbo.SisMoedaCotacao", "SubGrupoId");
            AddForeignKey("dbo.SisMoedaCotacao", "PlanoId", "dbo.SisPlano", "Id");
            AddForeignKey("dbo.SisMoedaCotacao", "SubGrupoId", "dbo.FatSubGrupo", "Id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.SisMoedaCotacao", "SubGrupoId", "dbo.FatSubGrupo");
            DropForeignKey("dbo.SisMoedaCotacao", "PlanoId", "dbo.SisPlano");
            DropIndex("dbo.SisMoedaCotacao", new[] { "SubGrupoId" });
            DropIndex("dbo.SisMoedaCotacao", new[] { "PlanoId" });
            DropColumn("dbo.SisMoedaCotacao", "SubGrupoId");
            DropColumn("dbo.SisMoedaCotacao", "PlanoId");
            RenameIndex(table: "dbo.SisMoedaCotacao", name: "IX_GrupoId", newName: "IX_FaturamentoGrupoId");
            RenameColumn(table: "dbo.SisMoedaCotacao", name: "GrupoId", newName: "FaturamentoGrupoId");
        }
    }
}
